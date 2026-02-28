using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text.Json;
using System.Text.Json.Serialization;
using CSharpCodeReorganizer.Core;
using CSharpCodeReorganizer.Core.MemberData;

namespace CSharpCodeReorganizer.ConsoleTool;

public class CommandFactory
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters =
        {
            new JsonStringEnumConverter(),
            new FrozenDictionaryConverter<MemberType, int>(),
            new FrozenDictionaryConverter<AccessModifier, int>(),
            new FrozenDictionaryConverter<AdditionalModifier, int>()
        },
        WriteIndented = true,
    };

    public static CommandFactory Default { get; } = new();

    private static FileInfo[]? ParseInputFilesOption(ArgumentResult result)
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
                    ? $"One or more input files paths are invalid."
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

    private static FileInfo[]? ParseOutputFilesOption(ArgumentResult result)
    {
        switch (result.Tokens)
        {
            case { Count: > 0 } tokens:
                var filesInfo = new FileInfo[tokens.Count];
                var badPath = new List<string>();
                foreach (var (index, token) in tokens.Index())
                {
                    var fileInfo = new FileInfo(token.Value);

                    if (fileInfo.Directory is null || fileInfo.Directory.Exists)
                        filesInfo[index] = fileInfo;
                    else
                        badPath.Add(token.Value);
                }

                result.ErrorMessage = badPath.Count > 0
                    ? $"One or more output files paths are invalid."
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

    private static FileInfo? ParseReorganizerConfigOption(ArgumentResult result)
    {
        switch (result.Tokens)
        {
            case { Count: 1 } tokens:
                var path = tokens.First().Value;
                var isExists = File.Exists(path);
                result.ErrorMessage = isExists
                                    ? null
                                    : $"Configuration file path {path} is invalid.";

                return isExists ? new FileInfo(path) : null;
            case { Count: 0 }:
                result.ErrorMessage = "A configuration file must be specified.";
                return null;
            case { Count: > 0 }:
                result.ErrorMessage = "Only one configuration file can be specified.";
                return null;
            default:
                throw new NotImplementedException();
        }
    }

    static async Task ReorganizeFile(FileInfo inputFile, FileInfo? outputFile, CsReorganizer csReorganizer)
    {
        await Task.Yield();
        var inPath = inputFile.FullName;
        var outPath = outputFile is null ? inPath : outputFile.FullName;

        var text = await File.ReadAllTextAsync(inPath);

        var reorganizedText = csReorganizer.Reorganize(text);

        await File.WriteAllTextAsync(outPath, reorganizedText);
    }

    private static async Task CommandHandler(FileInfo[] inputFiles, FileInfo[]? outputPaths, FileInfo? configFileInfo)
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

        var configParameters = new CsReorganizerParameters(new(), new());

        if (configFileInfo is not null)
        {
            using var file = configFileInfo.OpenRead();
            configParameters = await JsonSerializer.DeserializeAsync<CsReorganizerParameters>(file, _serializerOptions);
        }

        var reorganizer = new CsReorganizer(configParameters);

        var tasks = pathPairs.Select(((FileInfo Input, FileInfo? Output) files) =>
            ReorganizeFile(files.Input, files.Output, reorganizer)
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
                                                       parseArgument: ParseInputFilesOption)
        {
            IsRequired = true,
            AllowMultipleArgumentsPerToken = true,
        };

        var outputFilesOption = new Option<FileInfo[]?>(name: "--output-files",
                                                        description: "Output paths.",
                                                        parseArgument: ParseOutputFilesOption)
        {
            AllowMultipleArgumentsPerToken = true,
        };

        var reorganizerConfigOption = new Option<FileInfo?>(name: "--config-file",
                                                            description: "Reorganize order configuration file.",
                                                            parseArgument: ParseReorganizerConfigOption);

        var rootCommand = new RootCommand("Reorganize command")
        {
            inputFilesOption,
            outputFilesOption,
            reorganizerConfigOption
        };

        rootCommand.SetHandler(CommandHandler, inputFilesOption, outputFilesOption, reorganizerConfigOption);

        return rootCommand;
    }
}