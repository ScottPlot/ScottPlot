namespace CodeAnalysis.HtmlReports;

public static class HtmlTemplate
{
    public static string WrapInPico(string content, string? title = null)
    {
        return @"<!doctype html>
<html lang='en'>
  <head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <link rel='stylesheet' href='https://unpkg.com/@picocss/pico@1.*/css/pico.min.css'>
    <title>{{TITLE}}</title>
  </head>
  <body>
    <main class='container'>
      {{CONTENT}}
    </main>
  </body>
</html>".Replace("{{TITLE}}", title).Replace("{{CONTENT}}", content);
    }

    public static string WrapInBootstrap(string content, string? title = null)
    {
        return @"<!doctype html>
<html lang='en'>
  <head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <title>{{TITLE}}</title>
    <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css'>
  </head>
  <body>
    <div class='container'>{{CONTENT}}</div>
    <script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js'></script>
  </body>
</html>".Replace("{{TITLE}}", title).Replace("{{CONTENT}}", content);
    }
}
