using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace CreateZipFiles
{
    class Program
    {
        private const string SourceArchive = @"C:\Users\chris\Downloads\ProfessionalCSharp7-master.zip";
        private const string SourceTempFolder = @"c:\temp\procsharp7";

        private readonly static string SourcesFolder = Path.Combine(SourceTempFolder, "ProfessionalCSharp7-master");

        private const string ZipFolder = @"c:\temp\zipchapters";

       // private const string ResultFolder = @"c:\temp\results";


        private static Dictionary<string, string> ZipFileMapping;

        static void Main()
        {
            InitZipFileMapping();
            Run();
        }

        private static void Run()
        {
            if (!UncompressFiles()) return;
            CreateZipFilesForChapters();

            // the following is only needed for multiple versions per zip file, e.g. vs2015 & vs2017
            //if (!Directory.Exists(ResultFolder))
            //{
            //    Directory.CreateDirectory(ResultFolder);
            //}

            //foreach (var zipFile in ZipFileMapping.Keys)
            //{
            //    string zipPath = Path.Combine(ResultFolder, zipFile);

            //    string file1 = Path.Combine(ZipFolder, $"{ZipFileMapping[zipFile]}-vs2015.zip");
            //    string file2 = Path.Combine(ZipFolder, $"{ZipFileMapping[zipFile]}-vs2017.zip");

            //    using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            //    {
            //        if (File.Exists(file1))
            //        {
            //            archive.CreateEntryFromFile(file1, Path.GetFileName(file1));
            //        }
            //        else
            //        {
            //            Console.WriteLine($"{file1} does not exist");
            //        }

            //        if (File.Exists(file2))
            //        {
            //            archive.CreateEntryFromFile(file2, Path.GetFileName(file2));
            //        }
            //        else
            //        {
            //            Console.WriteLine($"{file2} does not exist");
            //        }
            //    }

            //    Console.WriteLine($"created {zipPath}");

            //}
        }

        private static void CreateZipFilesForChapters()
        {
            if (!Directory.Exists(ZipFolder))
            {
                Directory.CreateDirectory(ZipFolder);
            }

            foreach (var zipFile in ZipFileMapping.Keys)
            {
                string foldersForChapter = ZipFileMapping[zipFile];

                // multiple folders for one zip file
                if (foldersForChapter.Contains(";"))
                {
                    string[] folders = foldersForChapter.Split(";");
                    // CreateZipFile($"{folders[0]}.zip", folders);
                    CreateZipFile($"{zipFile}", folders);
                }
                else
                {
                    CreateZipFile($"{zipFile}", foldersForChapter);
                    // CreateZipFile($"{foldersForChapter}.zip", foldersForChapter);
                }
            }

            void CreateZipFile(string zipFile, params string[] sourceFolders)
            {
                foreach (var sourceFolder in sourceFolders)
                {
                    string sourcePath = Path.Combine(SourcesFolder, sourceFolder);
                    if (!Directory.Exists(sourcePath))
                    {
                        Console.WriteLine($"{sourcePath} does not exist");
                        return;
                    }
                }

                if (sourceFolders.Length == 1)
                {
                    string sourcePath = Path.Combine(SourcesFolder, sourceFolders[0]);
                    string zipPath = Path.Combine(ZipFolder, zipFile);
                    ZipFile.CreateFromDirectory(sourcePath, zipPath);
                    Console.WriteLine($"created {zipPath}");
                }
                else
                {
                    string tempFolderPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    Directory.CreateDirectory(tempFolderPath);
                    foreach (var sourceFolder in sourceFolders)
                    {
                        string subFolder = Path.Combine(tempFolderPath, sourceFolder);
                        string sourcePath = Path.Combine(SourcesFolder, sourceFolder);
                        string destinationPath = Path.Combine(tempFolderPath, sourceFolder);
                        Directory.Move(sourcePath, destinationPath);       
                    }

                    string zipPath = Path.Combine(ZipFolder, zipFile);
                    ZipFile.CreateFromDirectory(tempFolderPath, zipPath);
                    Console.WriteLine($"created {zipPath}");

                    // move back - some files might be needed again
                    foreach (var sourceFolder in sourceFolders)
                    {
                        string subFolder = Path.Combine(tempFolderPath, sourceFolder);
                        string sourcePath = Path.Combine(SourcesFolder, sourceFolder);
                        string destinationPath = Path.Combine(tempFolderPath, sourceFolder);
                        Directory.Move(destinationPath, sourcePath);
                    }

                    Directory.Delete(tempFolderPath);
                    Console.WriteLine($"deleted {tempFolderPath}");
                }
            }
        }

        private static bool UncompressFiles()
        {
            Console.WriteLine("Uncompressing files...");
            if (Directory.Exists(SourceTempFolder))
            {
                Console.WriteLine($"delete {SourceTempFolder} before running this app");
                return false;
            }

            Directory.CreateDirectory(SourceTempFolder);

            Console.WriteLine("extracting zip with all sources...");
            ZipFile.ExtractToDirectory(SourceArchive, SourceTempFolder);

            Console.WriteLine($"Uncompressed files to {SourceTempFolder}");
            return true;
        }

        private static void InitZipFileMapping()
        {
            ZipFileMapping = new Dictionary<string, string>()
            {
                ["01_code.zip"] = "HelloWorld",
                ["02_code.zip"] = "CoreCSharp",
                ["03_code.zip"] = "ObjectsAndTypes",
                ["04_code.zip"] = "ObjectOrientation",
                ["05_code.zip"] = "Generics",
                ["06_code.zip"] = "OperatorsAndCasts",
                ["07_code.zip"] = "Arrays",
                ["08_code.zip"] = "Delegates",
                ["09_code.zip"] = "StringsAndRegularExpressions",
                ["10_code.zip"] = "Collections",
                ["11_code.zip"] = "SpecialCollections",
                ["12_code.zip"] = "LINQ",
                ["13_code.zip"] = "FunctionalProgramming",
                ["14_code.zip"] = "ErrorsAndExceptions",
                ["15_code.zip"] = "Async",
                ["16_code.zip"] = "ReflectionAndDynamic",
                ["17_code.zip"] = "Memory",
                ["18_code.zip"] = "VisualStudio",
                ["19_code.zip"] = "Libraries",
                ["20_code.zip"] = "DependencyInjection",
                ["21_code.zip"] = "Tasks",
                ["22_code.zip"] = "FilesAndStreams",
                ["23_code.zip"] = "Networking",
                ["24_code.zip"] = "Security",
                ["25_code.zip"] = "Composition",
                ["26_code.zip"] = "XMLandJSON",
                ["27_code.zip"] = "ADONet",
                ["28_code.zip"] = "EFCore",
                ["29_code.zip"] = "Localization",
                ["30_code.zip"] = "Tests",
                ["31_code.zip"] = "Diagnostics",
                ["32_code.zip"] = "ASPNETCore",
                ["33_code.zip"] = "MVC",
                ["34_code.zip"] = "API",
                ["35_code.zip"] = "SignalRAndWebHooks",
                ["36_code.zip"] = "BotAndCognitive",
                ["37_code.zip"] = "Windows",
                ["38_code.zip"] = "Patterns;PatternsXamarinShared",
                ["39_code.zip"] = "Styles",
                ["40_code.zip"] = "AdvancedWindows",
                ["41_code.zip"] = "Xamarin;PatternsXamarinShared"
            };
        }
    }
}
