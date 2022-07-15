using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.IO.Compression;
using FluentFTP;
using Microsoft.Extensions.Configuration;

namespace DemoPackager
{
    /// <summary>
    /// This application builds, zips, and deploys the demos to the ScottPlot.NET website
    /// </summary>
    internal class Program
    {
        private static string RepoFolder;
        private static string DemoOutputFolder => Path.Combine(RepoFolder, "src/demo/packages");

        static void Main()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            RepoFolder = Path.GetFullPath(Path.Combine(assemblyFolder, "../../../../../../"));

            if (!Directory.Exists(RepoFolder))
                throw new InvalidOperationException($"Repository folder does not exist: {RepoFolder}");

            if (!File.Exists(Path.Combine(RepoFolder, "LICENSE")))
                throw new InvalidOperationException($"Folder is not top level of this repository: {RepoFolder}");

            Directory.SetCurrentDirectory(assemblyFolder);
            if (!File.Exists("Images/bicycle.png"))
                throw new InvalidOperationException($"Working directory does not contain images: {Path.GetFullPath("./")}");

            Console.WriteLine("This application will build and ZIP the ScottPlot demo applications.");
            Console.WriteLine("It is only intended to be run the ScottPlot website administrators.");
            Console.WriteLine();
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            ResetOutputFolder();
            GenerateRecipes();
            BuildAndZip(Path.Combine(RepoFolder, @"src\ScottPlot4\ScottPlot.Demo\ScottPlot.Demo.WinForms"), true, "ScottPlot-Demo-WinForms.zip");
            BuildAndZip(Path.Combine(RepoFolder, @"src\ScottPlot4\ScottPlot.Demo\ScottPlot.Demo.WPF"), true, "ScottPlot-Demo-WPF.zip");
            BuildAndZip(Path.Combine(RepoFolder, @"src\ScottPlot4\ScottPlot.Demo\ScottPlot.Demo.Avalonia"), true, "ScottPlot-Demo-Avalonia.zip");

            Console.WriteLine();
            Console.WriteLine("ZIP files created. Press ENTER to upload them to the ScottPlot website...");
            Console.ReadLine();
            FtpUpload("/scottplot.net/public_html/demos");
        }

        static void GenerateRecipes()
        {
            Console.WriteLine("Generating cookbook recipes from source code...");
            string cookbookFolder = Path.Combine(RepoFolder, "src/ScottPlot4/ScottPlot.Cookbook");
            string json = ScottPlot.Cookbook.RecipeJson.Generate(cookbookFolder);
            File.WriteAllText("recipes.json", json);
        }

        static void ResetOutputFolder()
        {
            if (Directory.Exists(DemoOutputFolder))
                Directory.Delete(DemoOutputFolder, true);
            Directory.CreateDirectory(DemoOutputFolder);
            Console.WriteLine($"Saving output in: {DemoOutputFolder}");
        }

        static void BuildAndZip(string projectFolder, bool msbuild, string zipFileName)
        {
            if (!Directory.Exists(projectFolder))
                throw new DirectoryNotFoundException(projectFolder);

            Console.WriteLine();
            Console.WriteLine($"Building {Path.GetFileName(projectFolder)}...");

            Console.WriteLine("Deleting previous builds...");
            string releaseFolder = Path.Combine(projectFolder, @"bin\Release");
            if (Directory.Exists(releaseFolder))
                Directory.Delete(releaseFolder, true);

            string cmd = msbuild
                ? $"msbuild \"{projectFolder}\" /p:Configuration=Release"
                : $"dotnet build \"{projectFolder}\" --configuration Release";

            Process pProcess = new();
            pProcess.StartInfo.FileName = cmd.Split(" ", 2)[0];
            pProcess.StartInfo.Arguments = cmd.Split(" ", 2)[1];
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.Start();
            string strOutput = pProcess.StandardOutput.ReadToEnd();
            pProcess.WaitForExit();

            if (pProcess.ExitCode != 0)
            {
                Console.WriteLine(cmd);
                Console.WriteLine(strOutput);
                throw new InvalidOperationException($"exited with code {pProcess.ExitCode}");
            }

            releaseFolder = Directory.GetDirectories(releaseFolder)[0];
            Console.WriteLine("Coping recipes.json...");
            File.Copy("recipes.json", Path.Combine(releaseFolder, "recipes.json"), overwrite: true);

            Console.WriteLine($"Creating {zipFileName}...");
            string zipFilePath = Path.Combine(DemoOutputFolder, zipFileName);
            ZipFile.CreateFromDirectory(releaseFolder, zipFilePath);
        }

        /// <summary>
        /// Upload contents of demo output folder to the given FTP URL
        /// </summary>
        static void FtpUpload(string remoteFilePath)
        {
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            foreach (var child in config.GetChildren())
                Environment.SetEnvironmentVariable(child.Key, child.Value);

            Console.WriteLine();
            Console.WriteLine("FTP Connecting...");
            string username = Environment.GetEnvironmentVariable("FTP_USERNAME");
            string password = Environment.GetEnvironmentVariable("FTP_PASSWORD");
            using var client = new FtpClient("scottplot.net", username, password);
            client.EncryptionMode = FtpEncryptionMode.Explicit;
            client.ValidateAnyCertificate = true;
            client.Connect();

            Console.WriteLine("FTP Uploading...");
            client.UploadFiles(Directory.GetFiles(DemoOutputFolder), remoteFilePath);

            Console.WriteLine("FTP Complete!");
        }
    }
}
