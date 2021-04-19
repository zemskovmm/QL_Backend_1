using System;
using System.IO;
using System.Linq;
using CommandLine;

namespace QuartierLatin.Importer
{
    interface ICommandLineCommand
    {
        int Execute();
    }
    
    class Program
    {
        static int Main(string[] args) =>
            Parser.Default.ParseArguments(args, typeof(ICommandLineCommand)
                    .Assembly.GetTypes().Where(t => !t.IsAbstract && typeof(ICommandLineCommand).IsAssignableFrom(t))
                    .ToArray())
                .MapResult(opt => ((ICommandLineCommand) opt).Execute(),
                    errors => 1);
    }
}