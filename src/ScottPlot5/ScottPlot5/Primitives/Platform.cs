namespace ScottPlot;

public static class Platform
{
    private enum OS
    {
        Windows,
        Linux,
        MacOS,
        Other,
    };

    private static OS ThisOS = GetOS();

    private static OS GetOS()
    {
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            return OS.Windows;

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            return OS.Linux;

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            return OS.MacOS;

        return OS.Other;
    }

    /// <summary>
    /// Launch a web browser to a URL using a command appropriate for the operating system
    /// </summary>
    public static void LaunchWebBrowser(string url)
    {
        switch (ThisOS)
        {
            case OS.Windows:
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                break;
            case OS.Linux:
                Process.Start("xdg-open", url);
                break;
            case OS.MacOS:
                Process.Start("open", url);
                break;
            default:
                throw new InvalidOperationException("Cannot launch a web browser on this OS");
        }
    }

    public static void LaunchFile(string filePath)
    {
        filePath = Path.GetFullPath(filePath);
        ProcessStartInfo psi = new(filePath) { UseShellExecute = true };
        Process.Start(psi);
    }
}
