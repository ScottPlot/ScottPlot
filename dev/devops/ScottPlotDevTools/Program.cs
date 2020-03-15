using System;

namespace ScottPlotDevTools
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    Console.WriteLine("ERROR: argument required.");
                    ShowHelp();
                    break;
                case 1:
                    switch (args[0])
                    {
                        case "incrimentVersion": IncrimentVersion(); break;
                        case "makeCookbook": MakeCookbook(); break;
                        case "makeDemo": MakeDemo(); break;
                        default: Console.WriteLine("ERROR: unknown command."); ShowHelp(); break;
                    }
                    break;
                case 2:
                    switch (args[0])
                    {
                        case "setVersion": if (args.Length == 2) SetVersion(args[1]); else ShowHelp(); break;
                    }
                    break;
            }
        }

        static void IncrimentVersion()
        {
            Console.WriteLine("incrimenting version...");
        }

        static void SetVersion(string newVersion)
        {
            Console.WriteLine("setting version...");
        }

        static void MakeCookbook()
        {
            Console.WriteLine("making cookbook...");
        }

        static void MakeDemo()
        {
            Console.WriteLine("making demo...");
        }

        static void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Command Line Arguments:");
            Console.WriteLine();
            Console.WriteLine(" -incrimentVersion");
            Console.WriteLine(" -setVersion 1.2.3");
            Console.WriteLine(" -makeCookbook");
            Console.WriteLine(" -makeDemo");
        }
    }
}
