using System;
using System.Windows.Forms;

namespace ScottPlot.Sandbox.WinForms;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
#if NET5_0_OR_GREATER
        ApplicationConfiguration.Initialize();
#endif
        Application.Run(new Form1());
    }
}
