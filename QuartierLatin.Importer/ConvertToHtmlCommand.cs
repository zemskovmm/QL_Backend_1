using System;
using System.Diagnostics;
using System.IO;
using CommandLine;

namespace QuartierLatin.Importer
{
    [Verb("convert-html", HelpText = "Convert doc/docx documents to html format")]
    // ReSharper disable once UnusedType.Global
    class ConvertToHtmlCommand: ICommandLineCommand
    {
        [Option('i', "input", Required = true)] public string InputDirectory { get; set; }
        [Option('o', "output", Required = true)] public string OutputDirectory { get; set; }



        static void ConvertFile(string file, string output)
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "ql-convert", Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            try
            {
                Console.WriteLine($"Converting {file} to {output}");
                var tempIn = Path.Combine(tempDir, "file" + Path.GetExtension(file));
                var tempOut = Path.Combine(tempDir, "file.htm");
                File.Copy(file, tempIn);
                var cmdline = $"--headless --convert-to htm:HTML --outdir {tempDir} {tempIn}";
                Console.WriteLine($"$ soffice {cmdline}");
                using (var p = Process.Start("soffice", cmdline))
                    p.WaitForExit();
                File.Copy(tempOut, output);
            }
            finally
            {
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch
                {
                    Console.WriteLine("Unable to delete " + tempDir);
                }
            }
            
        }

       
        static void Convert(string directory, string output)
        {
            foreach (var dir in Directory.GetDirectories(directory))
            {
                var outputDir = Path.Combine(output, Path.GetFileName(dir));
                foreach (var file in Directory.GetFiles(dir))
                {
                    if (!file.ToLowerInvariant().EndsWith(".docx"))
                        continue;
                    
                    var fileName = Path.GetFileName(file);
                    var parts = fileName.Split(' ');
                    if (!int.TryParse(parts[0], out var _))
                    {
                        Console.WriteLine("Skipping " + file);
                        continue;
                    }

                    Directory.CreateDirectory(outputDir);
                    var outputFile = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(fileName) + ".html");
                    ConvertFile(file, outputFile);
                }
            }
        }
        
        public int Execute()
        {
            Convert(InputDirectory, OutputDirectory);   
            return 0;
        }
    }
}