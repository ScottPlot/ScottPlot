using System;
using Eto.Forms;

namespace ScottPlot.Demo.Eto
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            new Application(global::Eto.Platform.Detect).Run(new FormMain());
        }
    }
}
