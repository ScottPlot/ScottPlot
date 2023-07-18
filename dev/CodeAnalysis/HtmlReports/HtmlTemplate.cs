namespace CodeAnalysis.HtmlReports;

public static class HtmlTemplate
{
    public static string WrapInBootstrap(string content, string? title = null)
    {
        return @"<!doctype html>
<html lang='en'>
  <head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <title>{{TITLE}}</title>
    <link rel='icon' href='https://scottplot.net/favicon.ico' sizes='any'>
    <link rel='icon' href='https://scottplot.net/images/brand/favicon.svg' type='image/svg+xml'>
    <meta name='theme-color' content='#67217A' />
    <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css'>
    <style>
    a {text-decoration: none;}
    a:hover {text-decoration: underline;}
    h2 {margin-top: 2em;}
    .container{max-width: 1000px;}
    </style>
  </head>
  <body>
    <div style='background-color: #67217a;'>
        <div class='container'>
            <div class='d-flex py-4'>
	            <div class='ms-3 me-3 my-1'>
		            <a href='https://scottplot.net'>
			            <img class='' src='https://scottplot.net/images/brand/favicon.svg' width='56' height='56'>
		            </a>
	            </div>
	            <div class='mt-1'>
		            <a class='lh-1 text-light' href='https://scottplot.net' style='font-size: 1.8em;'>
			            ScottPlot.NET
		            </a>
                    <div class='font-monospace' style='color: #9a4993; font-size: 1.15em;'>
                        Developer Website
                    </div>
	            </div>
            </div>
        </div>
    </div>
    <div class='container my-5'>{{CONTENT}}</div>
    <script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js'></script>
  </body>
</html>".Replace("{{TITLE}}", title).Replace("{{CONTENT}}", content);
    }
}
