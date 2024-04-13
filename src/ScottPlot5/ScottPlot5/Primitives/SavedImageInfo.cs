namespace ScottPlot;

public class SavedImageInfo
{
    public string Path { get; }
    public int FileSize { get; }
    private RenderDetails RenderDetails;

    public SavedImageInfo(string path, int fileSize)
    {
        Path = System.IO.Path.GetFullPath(path);
        FileSize = fileSize;
    }

    public SavedImageInfo WithRenderDetails(RenderDetails renderDetails)
    {
        RenderDetails = renderDetails;
        return this;
    }

    public void LaunchFile() => Platform.LaunchFile(Path);

    public void LaunchInBrowser(double refresh = 3)
    {
        string html = ImageHtmlTemplate;
        html = html.Replace("{{ REFRESH_MSEC }}", $"{refresh * 1000}");
        html = html.Replace("{{ WIDTH }}", $"{RenderDetails.FigureRect.Width}");
        html = html.Replace("{{ HEIGHT }}", $"{RenderDetails.FigureRect.Height}");
        html = html.Replace("{{ VERSION }}", Version.LongString);
        html = html.Replace("{{ FILE }}", Path);
        html = html.Replace("{{ FILENAME }}", System.IO.Path.GetFileName(Path));

        string htmlFilePath = Path + "-viewer.html"; // TODO: use a temporary file
        File.WriteAllText(htmlFilePath, html);
        Platform.LaunchFile(htmlFilePath);
    }

    private readonly string ImageHtmlTemplate = """
        <!doctype html>
        <html lang="en">

        <head>
            <meta charset="utf-8">
            <meta name="viewport" content="width=device-width, initial-scale=1">
            <link rel="icon" href="https://scottplot.net/favicon.ico" sizes="any">
            <title>ScottPlot {{ FILENAME }}</title>
            <style>
                body {
                    padding: 0;
                    margin: 0;
                    text-align: center;
                    background-color: #f0f0f0;
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                }

                img {
                    background-color: white;
                    max-width: 80%;
                    margin: 5em auto 3em auto;
                    display: block;
                    box-shadow: 0px 0px 20px 10px rgba(0, 0, 0, 0.2);
                }

                a {
                    text-decoration: none;
                    color: black;
                }

                a:hover {
                    text-decoration: underline;
                }
            </style>

            <script language="javascript">
                function toggle(){
                    const url = isLive() ? '?' : '?refresh={{ REFRESH_MSEC }}';
                    window.location.href = url;
                }
        
                function isLive(){
                    const urlParams = new URLSearchParams(window.location.search);
                    const refreshMsec = urlParams.get('refresh');
                    return refreshMsec > 0;
                }
            </script>

        </head>

        <body>
            <a href='{{ FILE }}'><img src="{{ FILE }}" width="{{ WIDTH }}" height="{{ HEIGHT }}"></a>
            <div style='line-height: 150%;'>
                <div>{{ FILENAME }} ({{ WIDTH }}x{{ HEIGHT }}) <script>document.write(new Date().toLocaleTimeString());</script></div>
                <div><input id="liveCheckbox" type="checkbox" onclick="toggle();"> auto-refresh</div>
                <div style='opacity: .5;'><a href='https://scottplot.net'>{{ VERSION }}</a></div>
            </div>
        </body>

        <script language="javascript">
            document.getElementById('liveCheckbox').checked = isLive();
            const urlParams = new URLSearchParams(window.location.search);
            const refreshMsec = urlParams.get('refresh');
            if (refreshMsec > 0){
                setTimeout(function(){
                    window.location.reload(1);
                }, refreshMsec);
            }
        </script>

        </html>
        """;
}
