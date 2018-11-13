using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ChangeFileContent
{
    class Program
    {
        static void Main(string[] args)
        {
            ChangeFileContent(@"C:\temp\20480-Programming-in-HTML5-with-JavaScript-and-CSS3");
        }

        private static void ChangeFileContent(string path)
        {
            var files = Directory.EnumerateFiles(path, "Web.config", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.Contains("Views"))
                {
                    CheckFile(file);
                }
            }
        }

        private static void CheckFile(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            var newLines = new List<string>();
            foreach (var line in lines)
            {
                string newLine = ReplaceContent(line, "System.Web.WebPages.Razor, Version=1.0.0.0", "System.Web.WebPages.Razor, Version=3.0.0.0");
                newLines.Add(newLine);
            }
            Console.WriteLine($"changing file {filename}");
            File.WriteAllLines(filename, newLines.ToArray());
        }

        private static string ReplaceContent(string line, string existingContent, string newContent)
        {
            if (!line.Contains(existingContent)) return line;

            Console.WriteLine($"content to replace in line {line}");
            return line.Replace(existingContent, newContent);
        }
    }
}
