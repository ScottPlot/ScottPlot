using System;
using System.Diagnostics;

namespace ScottPlotDevTools
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"ScottPlot {ScottPlot.Tools.GetVersionString()}");

            //args = new string[] { "-incrimentVersion" };
            //args = new string[] { "-makeCookbook" };
            //args = new string[] { "-makeDemo" };
            ProcessArguments(args);
        }

        static void ProcessArguments(string[] args)
        {
            string command = "help";

            if (args.Length >= 1)
                command = args[0].Trim('-');

            switch (command)
            {
                case "help": ShowHelp(); break;
                case "incrimentVersion": IncrimentVersion(); break;
                case "makeCookbook": MakeCookbook(); break;
                case "makeDemo": MakeDemo(); break;
                default: Console.WriteLine("ERROR: unknown command."); ShowHelp(); break;
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Command Line Arguments:");
            Console.WriteLine();
            Console.WriteLine(" -incrimentVersion");
            Console.WriteLine(" -makeCookbook");
            Console.WriteLine(" -makeDemo");
        }

        static void IncrimentVersion()
        {
            Console.WriteLine("incrimenting version...");

            string[] projectPaths = {
                    "../../../../../../src/ScottPlot/ScottPlot.csproj",
                    "../../../../../../src/ScottPlot.WinForms/ScottPlot.WinForms.NUGET.csproj",
                    "../../../../../../src/ScottPlot.WPF/ScottPlot.WPF.NUGET.csproj",
                    "../../../../../../src/ScottPlot.Avalonia/ScottPlot.Avalonia.NUGET.csproj"
                 };

            foreach (string projectPathRel in projectPaths)
            {
                string projectPath = System.IO.Path.GetFullPath(projectPathRel);
                if (!System.IO.File.Exists(projectPath))
                    throw new ArgumentException($"file not found: {projectPath}");

                var projVersion = new ProjectFileVersion(projectPath);
                string oldVersion = projVersion.version.ToString();
                projVersion.Incriment();
                string newVersion = projVersion.version.ToString();
                Console.WriteLine($"{oldVersion} -> {newVersion} ({projVersion.name})");
                projVersion.Save();
            }
        }

        static void MakeCookbook()
        {
            Console.WriteLine("making cookbook...");
            Stopwatch stopwatch = Stopwatch.StartNew();
            string outputFolder = System.IO.Path.GetFullPath($"./{ScottPlot.Tools.GetVersionString()}");
            var reportGeneratpr = new ScottPlot.Demo.ReportGenerator(outputFolder: outputFolder);

            Console.WriteLine($"Preparing output folder...");
            reportGeneratpr.ClearFolders();

            Console.Write("Generating cookbook figures");
            var recipes = ScottPlot.Demo.Reflection.GetPlots();
            foreach (var recipe in recipes)
            {
                Console.Write(".");
                reportGeneratpr.CreateImage(recipe);
            }
            Console.WriteLine();

            Console.WriteLine($"Creating reports...");
            reportGeneratpr.MakeReports();

            double elapsedSec = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;
            Console.WriteLine($"Cookbook finished ({recipes.Length} figures generated in {elapsedSec:0.00} seconds)");
            Console.WriteLine(outputFolder);

            // launch the folder
            System.Diagnostics.Process.Start("explorer.exe", outputFolder);
        }

        static void CleanProject(string projectName)
        {
            string projectPath = $"../../../../../../src/{projectName}/{projectName}.csproj";
            projectPath = System.IO.Path.GetFullPath(projectPath);
            if (!System.IO.File.Exists(projectPath))
                throw new ArgumentException($"file not found: {projectPath}");
            Console.WriteLine($"\nCleaning: {projectName}...");
            RunCommand($"dotnet clean {projectPath} --verbosity quiet --configuration Release");

            string binPath = System.IO.Path.GetDirectoryName(projectPath) + "/bin";
            if (System.IO.Directory.Exists(binPath))
            {
                foreach (string binFolder in System.IO.Directory.GetDirectories(binPath))
                    System.IO.Directory.Delete(binFolder, true);

                foreach (string binFile in System.IO.Directory.GetFiles(binPath))
                    System.IO.File.Delete(binFile);
            }
        }

        static void BuildProject(string projectName)
        {
            string projectPath = $"../../../../../../src/{projectName}/{projectName}.csproj";
            projectPath = System.IO.Path.GetFullPath(projectPath);
            if (!System.IO.File.Exists(projectPath))
                throw new ArgumentException($"file not found: {projectPath}");
            Console.WriteLine($"\nBuilding: {projectName}...");
            RunCommand($"dotnet build {projectPath} --verbosity quiet --configuration Release");
            Console.WriteLine();
        }

        static void RunCommand(string command)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command) { UseShellExecute = false, };
            Process process = Process.Start(processInfo);
            process.WaitForExit();
            if (process.ExitCode != 0)
                throw new OperationCanceledException($"ERROR: exit code {process.ExitCode}");
            process.Close();
        }

        static void MakeDemo()
        {
            string wpfOutputPath = "../../../../../../src/ScottPlot.Demo.WPF/bin/Release/net472";
            string demoSourceFolder = "../../../../../../src/ScottPlot.Demo";
            string wpfOutputSourcePath = System.IO.Path.Combine(wpfOutputPath, "source");
            string versionOutputPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(wpfOutputPath), ScottPlot.Tools.GetVersionString());
            versionOutputPath = System.IO.Path.GetFullPath(versionOutputPath);
            string zipFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(wpfOutputPath), $"ScottPlotDemo-{ScottPlot.Tools.GetVersionString()}.zip");

            Console.WriteLine("Deleting old binaries...");
            if (System.IO.Directory.Exists(wpfOutputPath))
                System.IO.Directory.Delete(wpfOutputPath, true);
            if (System.IO.Directory.Exists(versionOutputPath))
                System.IO.Directory.Delete(versionOutputPath, true);
            if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(wpfOutputPath)))
                foreach (string zipFilePathToDelete in System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(wpfOutputPath), "*.zip"))
                    System.IO.File.Delete(zipFilePathToDelete);

            CleanProject("ScottPlot.Demo.WPF");
            BuildProject("ScottPlot.Demo.WPF");

            // move things into an output folder named by version
            CopySource(demoSourceFolder, wpfOutputSourcePath);
            System.IO.Directory.CreateDirectory(versionOutputPath);
            string exeFolder = System.IO.Path.Combine(versionOutputPath, "ScottPlot Demo");
            System.IO.Directory.Move(wpfOutputPath, exeFolder);

            // copy in other useful files
            Console.WriteLine("Copying bundle files...");
            string bundleFolder = "../../../../../../dev/devops/build/bundle";
            foreach (string filePath in System.IO.Directory.GetFiles(bundleFolder))
            {
                string fileName = System.IO.Path.GetFileName(filePath);
                System.IO.File.Copy(filePath, System.IO.Path.Combine(versionOutputPath, fileName));
            }

            // delete extra files we dont need
            foreach (string filePath in System.IO.Directory.GetFiles(exeFolder))
            {
                string fileName = System.IO.Path.GetFileName(filePath);
                // if the user wants to debug, let them build their own demo from source
                if (fileName.EndsWith(".pdb") || fileName.Contains(".osx.") || fileName.EndsWith(".exe.config"))
                    System.IO.File.Delete(filePath);
            }

            // zip the output folder
            Console.WriteLine("Creating zip file...");
            System.IO.Compression.ZipFile.CreateFromDirectory(versionOutputPath, zipFilePath);

            Console.WriteLine("Demo build complete!");
            Console.WriteLine(versionOutputPath);

            // launch the folder
            System.Diagnostics.Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(wpfOutputPath));
        }

        static void CopySource(string sourcePath, string destPath)
        {
            // verify the source folder is correct
            sourcePath = System.IO.Path.GetFullPath(sourcePath);
            destPath = System.IO.Path.GetFullPath(destPath);

            string fileInDemoRoot = System.IO.Path.Join(sourcePath, "IPlotDemo.cs");
            if (!System.IO.File.Exists(fileInDemoRoot))
                throw new ArgumentException("File note found: " + fileInDemoRoot);
            else
                Console.WriteLine("Found source code folder");

            Console.WriteLine("Copying source files to output folder...");
            if (!System.IO.Directory.Exists(destPath))
                System.IO.Directory.CreateDirectory(destPath);

            foreach (string sourceFile in System.IO.Directory.GetFiles(sourcePath, "*.cs"))
            {
                string fileName = System.IO.Path.GetFileName(sourceFile);
                string outputFileName = System.IO.Path.Combine(destPath, fileName);
                System.IO.File.Copy(sourceFile, outputFileName, overwrite: true);
                //Console.WriteLine($"  {fileName}");
            }

            foreach (string sourceSubFolder in System.IO.Directory.GetDirectories(sourcePath))
            {
                string folderName = System.IO.Path.GetFileName(sourceSubFolder);
                //Console.WriteLine($"  {folderName}/");
                foreach (string sourceFile in System.IO.Directory.GetFiles(sourceSubFolder, "*.cs"))
                {
                    string fileName = System.IO.Path.GetFileName(sourceFile);
                    string outputFolder = System.IO.Path.Combine(destPath, folderName);
                    string outputFileName = System.IO.Path.Combine(outputFolder, fileName);
                    if (!System.IO.Directory.Exists(outputFolder))
                        System.IO.Directory.CreateDirectory(outputFolder);
                    System.IO.File.Copy(sourceFile, outputFileName, overwrite: true);
                    //Console.WriteLine($"    {fileName}");
                }
            }
        }
    }
}
