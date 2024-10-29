using System.CommandLine;

namespace CSharpCodeReorganizer.ConsoleTool;

class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = CommandFactory.Default.CreateReorganizerCommand();
        try
        {
            await rootCommand.InvokeAsync(args);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"ERROR: {ex.Message}");
            return 1;
        }
        return 0;
    }
}