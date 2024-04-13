using System.Text;

namespace CodeAnalysis.HtmlReports;

internal class TableBuilder
{
    StringBuilder Body = new();

    public void AddHeader(string[] items)
    {
        Body.AppendLine("<tr>");
        foreach (string item in items)
        {
            Body.AppendLine($"<th><strong>{item}</strong></th>");
        }
        Body.AppendLine("</tr>");
    }

    public void AddRow(string[] items)
    {
        Body.AppendLine("<tr>");
        foreach (string item in items)
        {
            Body.AppendLine($"<td>{item}</td>");
        }
        Body.AppendLine("</tr>");
    }

    public string GetHtml()
    {
        return $"<table>{Body}</table>";
    }
}
