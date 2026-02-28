using System.CommandLine;
using System.CommandLine.Parsing;
using CSharpCodeReorganizer.Core;

namespace CSharpCodeReorganizer.ConsoleTool;

public class CommandFactory
{
    public static CommandFactory Default { get; } = new();

    private FileInfo[]? ParseFileOption(ArgumentResult result)
    {
        switch (result.Tokens)
        {
            case { Count: > 0 } tokens:
                var filesInfo = new FileInfo[tokens.Count];
                var badPath = new List<string>();
                foreach (var (index, token) in tokens.Index())
                {
                    if (File.Exists(token.Value))
                        filesInfo[index] = new FileInfo(token.Value);
                    else
                        badPath.Add(token.Value);
                }

                result.ErrorMessage = badPath.Count > 0
                    ? $"One or more paths are invalid."
                      + Environment.NewLine
                      + string.Join(Environment.NewLine, badPath)
                    : null;

                return badPath.Count is 0 ? filesInfo : null;

            case { Count: 0 }:
                result.ErrorMessage = "At least one file must be specified.";
                return null;

            default:
                throw new NotImplementedException();
        }
    }

    async Task ReorganizeFile(FileInfo inputFile, FileInfo? outputFile)
    {
        await Task.Yield();
        var inPath = inputFile.FullName;
        var outPath = outputFile is null ? inPath : outputFile.FullName;

        var text = await File.ReadAllTextAsync(inPath);

        var reorganizedText = CsReorganizer.Default.Reorganize(text);

        await File.WriteAllTextAsync(outPath, reorganizedText);
    }

    private async Task CommandHandler(FileInfo[] inputFiles, FileInfo[]? outputPaths)
    {
        ArgumentNullException.ThrowIfNull(inputFiles);

        if (inputFiles is null)
        {
            Console.Error.WriteLine("No file specified.");
            return;
        }

        if ((outputPaths?.Length is not null and > 0) && inputFiles.Length != outputPaths.Length)
        {
            Console.Error.WriteLine("The number of input and output paths must be the same.");
            return;
        }

        var pathPairs = inputFiles.Zip(outputPaths?.Length is null or 0
                                        ? Enumerable.Repeat<FileInfo?>(null, inputFiles.Length)
                                        : outputPaths);

        var tasks = pathPairs.Select(((FileInfo Input, FileInfo? Output) files) =>
            ReorganizeFile(files.Input, files.Output)
            .ContinueWith((t) => Console.WriteLine(t.Exception is null
                                                    ? $"Processed {files.Input} -> {files.Output}"
                                                    : $"Failed to process {files.Input}: {t.Exception}")));

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public RootCommand CreateReorganizerCommand()
    {
        var inputFilesOption = new Option<FileInfo[]?>(name: "--input-files",
                                                       description: "Files to reorganize.",
                                                       isDefault: true,
                                                       parseArgument: ParseFileOption)
        {
            IsRequired = true,
            AllowMultipleArgumentsPerToken = true,
        };

        var outputFilesOption = new Option<FileInfo[]?>(name: "--output-files",
                                                        description: "Output paths.")
        {
            AllowMultipleArgumentsPerToken = true,
        };

        var rootCommand = new RootCommand("Reorganize command")
        {
            inputFilesOption,
            outputFilesOption,
        };

        rootCommand.SetHandler(CommandHandler, inputFilesOption, outputFilesOption);

        return rootCommand;
    }
}