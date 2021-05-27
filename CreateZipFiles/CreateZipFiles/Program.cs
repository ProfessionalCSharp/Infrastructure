using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace CreateZipFiles
{
    class Program
    {
        private const string SourceArchive = @"C:\Users\ChristianNagel\Downloads\ProfessionalCSharp2021-main.zip";
        private const string SourceTempFolder = @"c:\temp\procsharp2021";

        private readonly static string SourcesFolder = Path.Combine(SourceTempFolder, "ProfessionalCSharp2021-main");

        private const string ZipFolder = @"c:\temp\zipchapters";

       // private const string ResultFolder = @"c:\temp\results";


        private static Dictionary<string, string>? ZipFileMapping;

        static void Main()
        {
            ZipFileMapping = InitZipFileMapping();
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
            if (ZipFileMapping is null) throw new InvalidOperationException("Initialize ZipFileMapping first");

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

        private static Dictionary<string, string> InitZipFileMapping()
        {
            return new Dictionary<string, string>()
            {
                ["01_code.zip"] = "1_CS/HelloWorld",
                ["02_code.zip"] = "1_CS/CoreCSharp",
                ["03_code.zip"] = "1_CS/Types",
                ["04_code.zip"] = "1_CS/ObjectOrientation",
                ["05_code.zip"] = "1_CS/OperatorsAndCasts",
                ["06_code.zip"] = "1_CS/Arrays",
                ["07_code.zip"] = "1_CS/Delegates",
                ["08_code.zip"] = "1_CS/Collections",
                ["09_code.zip"] = "1_CS/LINQ",
                ["10_code.zip"] = "1_CS/ErrorsAndExceptions",
                ["11_code.zip"] = "1_CS/Tasks",
                ["12_code.zip"] = "1_CS/ReflectionAndSourceGenerators",
                ["13_code.zip"] = "1_CS/Memory",

                ["14_code.zip"] = "2_Libs/Libraries",
                ["15_code.zip"] = "2_Libs/DependencyInjectionAndConfiguration",
                ["16_code.zip"] = "2_Libs/LoggingAndMetrics",
                ["17_code.zip"] = "2_Libs/Parallel",
                ["18_code.zip"] = "2_Libs/FilesAndStreams",
                ["19_code.zip"] = "2_Libs/Networking",
                ["20_code.zip"] = "2_Libs/Security",
                ["21_code.zip"] = "2_Libs/EFCore",
                ["22_code.zip"] = "2_Libs/Localization",
                ["23_code.zip"] = "2_Libs/Tests",

                ["24_code.zip"] = "3_Web/ASPNETCore",
                ["25_code.zip"] = "3_Web/Services",
                ["26_code.zip"] = "3_Web/RazorAndMVC",
                ["27_code.zip"] = "3_Web/Blazor",
                ["28_code.zip"] = "3_Web/SignalR",

                ["29_code.zip"] = "4_Apps/Windows",
                ["30_code.zip"] = "4_Apps/Patterns",
                ["31_code.zip"] = "4_Apps/Styles",
            };
        }
    }
}
