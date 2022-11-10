using System.Text;

namespace ScottPlotCookbook.HtmlPages;

internal abstract class HtmlPageBase
{
    protected StringBuilder SB = new();

    private static string WrapPage(string content, string title) => @"<!doctype html>
        <html lang=""en"">
          <head>
            <meta charset=""utf-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
            <title>{{TITLE}}</title>
            <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/css/bootstrap.min.css"" rel=""stylesheet"" >
            <style>
            a {text-decoration: none;}
            a:hover {text-decoration: underline;}
            </style>
          </head>
          <body>
            <div class=""container"">
                <main>{{CONTENT}}</main>
            </div>
          </body>
        </html>"
        .Replace("{{TITLE}}", title)
        .Replace("{{CONTENT}}", content)
        .Replace("    ", "");

    protected void Save(string folder, string title, bool localFile = false)
    {
        string html = WrapPage(SB.ToString(), title);
        string filename = "index.html";

        if (localFile)
        {
            html = html.Replace("/#", "/index.html#");
            filename = "index.local.html";
        }

        string saveAs = Path.Combine(folder, filename);
        File.WriteAllText(saveAs, html);
        TestContext.WriteLine(saveAs);
    }
}
