using System.Text;

using CodeAnalysis.HtmlReports;

namespace CodeAnalysis;

public static class CodeReport
{
    public static void Generate(string repoRootPath, string saveAs = "code-report.html")
    {
        ProjectMetrics metrics = new(repoRootPath);

        StringBuilder sb = new();

        sb.AppendLine("<h1 align='center' style='margin-bottom: 0px;'>ScottPlot Code Metrics</h1>");
        sb.AppendLine($"<div align='center'>This project contains {metrics.GetLines()}</div>");
        sb.AppendLine($"<hr style='margin: 50px;' />");

        AddLinesOfCodeSection(sb, metrics);
        AddTodoSection(sb, metrics, "ScottPlot5");

        sb.AppendLine($"<div align='center' style='margin-top: 100px;'>Generated {DateTime.Now}</div>");
        sb.AppendLine("<div align='center' style='margin-top: 1em;'>" +
            "<a href='https://github.com/ScottPlot/ScottPlot/tree/main/dev/CodeAnalysis'>" +
            "https://github.com/ScottPlot/ScottPlot/tree/main/dev/CodeAnalysis</a></div>");

        saveAs = Path.GetFullPath(saveAs);
        string html = HtmlTemplate.WrapInBootstrap(sb.ToString(), "ScottPlot Code Metrics");
        Directory.CreateDirectory(Path.GetDirectoryName(saveAs)!);
        File.WriteAllText(saveAs, html);
        Console.WriteLine($"Wrote: {saveAs}");
    }

    private static void AddLinesOfCodeSection(StringBuilder sb, ProjectMetrics metrics)
    {
        sb.AppendLine("<h2 style='margin-bottom: 0px;'>Lines of Code</h2>");
        sb.AppendLine($"<li style='margin-top: 1em;'><b>ScottPlot 5: {metrics.GetLines("ScottPlot5")}</b></li>");
        sb.AppendLine($"<ul>");
        sb.AppendLine($"<li>Library: {metrics.GetLines("ScottPlot5", "ScottPlot5")}</li>");
        sb.AppendLine($"<li>Tests: {metrics.GetLines("ScottPlot5", "ScottPlot5 Tests")}</li>");
        sb.AppendLine($"<li>Cookbook: {metrics.GetLines("ScottPlot5", "ScottPlot5 Cookbook")}</li>");
        sb.AppendLine($"<li>Demos: {metrics.GetLines("ScottPlot5", "ScottPlot5 Demos")}</li>");
        sb.AppendLine($"</ul>");

        sb.AppendLine($"</ul>");
    }

    private static void AddTodoSection(StringBuilder sb, ProjectMetrics metrics, string folderFilter)
    {
        List<string> todoItems = [];
        foreach (string filePath in metrics.Files
            .Select(x => x.FilePath)
            .Where(x => x.Contains(folderFilter)))
        {
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].Contains("TO" + "DO:"))
                {
                    continue;
                }

                string message = lines[i].Split("TO" + "DO:")[1].Trim();
                string relativeFilePath = Path
                    .GetRelativePath(metrics.FolderPath, filePath)
                    .Replace("\\", "/");
                string fileUrl = $"https://github.com/ScottPlot/ScottPlot/tree/main/{relativeFilePath}#L{i + 1}";

                todoItems.Add($@"
          <tr>
            <td data-key=""fileName"">{Path.GetFileName(filePath)}</td>
            <td data-key=""line"">{i + 1}</td>
            <td data-key=""message"">{message}</td>
            <td><a href=""{fileUrl}"" class=""btn btn-sm btn-outline-primary"">View</a></td>
          </tr>");
            }
        }

        sb.AppendLine($"<h2 style='margin-bottom: .5em;'>{folderFilter} TODOs</h2>");

        sb.AppendLine($@"
<!-- TODO table (pure JS, bootstrap styles) -->
<div class=""card mb-4"">
  <div class=""card-body"">
    <div class=""d-flex justify-content-between align-items-center mb-2"">
      <button class=""btn btn-primary btn-lg"" type=""button"" data-bs-toggle=""collapse"" data-bs-target=""#todoSection"" aria-expanded=""false"" aria-controls=""todoSection"">
        Toggle TODOs List ({todoItems.Count})
      </button>
      <div id=""todoSection"" class=""collapse w-50"">
        <input id=""todoSearch"" class=""form-control form-control-sm"" placeholder=""Search TODO ..."">
      </div>
    </div>

    <div id=""todoSection"" class=""collapse table-responsive"">
      <table id=""todoTable"" class=""table table-hover table-sm align-middle table-striped "">
        <thead class=""table-light"">
          <tr>
            <th scope=""col"" class=""sortable"" data-key=""fileName"">File Name <span class=""sort-indicator""></span></th>
            <th scope=""col"" class=""sortable"" data-key=""line"">Line <span class=""sort-indicator""></span></th>
            <th scope=""col"" class=""sortable"" data-key=""message"">Message <span class=""sort-indicator""></span></th>
            <th scope=""col"">Source</th>
          </tr>
        </thead>
        <tbody>{string.Concat(todoItems)}
        </tbody>
      </table>
    </div>
  </div>
</div>

<style>
  /* Small style: sort indicator */
  .sortable {{ cursor: pointer; user-select: none; }}
  .sort-indicator {{ font-size: 0.8em; margin-left: 6px; color: #6c757d; }}
  .sorted-asc .sort-indicator::after {{ content: ""▲""; }}
  .sorted-desc .sort-indicator::after {{ content: ""▼""; }}
  /* Optional: Search Hit Highlight */
  .match {{ background: rgba(255, 235, 59, 0.12); }}
</style>

<script>
(function(){{
  const table = document.getElementById('todoTable');
  const tbody = table.querySelector('tbody');
  const headers = table.querySelectorAll('th.sortable');
  const searchInput = document.getElementById('todoSearch');

  // Current sorting status
  const state = {{
    key: null,
    dir: 1 // 1 = asc, -1 = desc
  }};

  // utils: detect numeric / date
  function parseCellValue(val){{
    if (val == null) return '';
    const s = String(val).trim();
    // date YYYY-MM-DD detect
    if (/^\d{{4}}-\d{{2}}-\d{{2}}/.test(s)) return new Date(s).getTime();
    // numeric
    const f = parseFloat(s.replace(/[,]/g,'')); // remove commas
    if (!isNaN(f) && isFinite(f) && /^-?\d/.test(s)) return f;
    return s.toLowerCase();
  }}

  // read table rows into array of {{el, values: {{key: value}}}}
  function readRows(){{
    return Array.from(tbody.rows).map(tr => {{
      const cells = {{}};
      tr.querySelectorAll('[data-key]').forEach(td=>{{
        cells[td.getAttribute('data-key')] = td.textContent.trim();
      }});
      return {{ el: tr, values: cells }};
    }});
  }}

  // sort rows and re-append
  function sortBy(key){{
    const rows = readRows();

    // toggle dir
    if (state.key === key) state.dir = -state.dir;
    else {{ state.key = key; state.dir = 1; }}

    // reset header classes
    headers.forEach(h => h.classList.remove('sorted-asc','sorted-desc'));

    // mark current header
    const cur = Array.from(headers).find(h => h.dataset.key === key);
    if (cur) cur.classList.add(state.dir === 1 ? 'sorted-asc' : 'sorted-desc');

    rows.sort((a,b) => {{
      const A = parseCellValue(a.values[key]);
      const B = parseCellValue(b.values[key]);
      if (A === B) return 0;
      return (A > B ? 1 : -1) * state.dir;
    }});

    // re-append in order
    const frag = document.createDocumentFragment();
    rows.forEach(r => frag.appendChild(r.el));
    tbody.appendChild(frag);
  }}

  // add click handlers to headers
  headers.forEach(h => {{
    h.addEventListener('click', () => sortBy(h.dataset.key));
    // allow keyboard activation
    h.setAttribute('tabindex', '0');
    h.addEventListener('keydown', (e) => {{ if (e.key === 'Enter') sortBy(h.dataset.key); }});
  }});

  // search/filter (simple text search across all cells)
  function filterRows(term){{
    const rows = Array.from(tbody.rows);
    const q = term.trim().toLowerCase();
    rows.forEach(tr => {{
      const text = tr.textContent.toLowerCase();
      const matched = q === '' || text.indexOf(q) !== -1;
      tr.style.display = matched ? '' : 'none';

      // optional: highlight matched rows (simple)
      if (matched && q !== '') {{
        // highlight matching cells
        Array.from(tr.cells).forEach(td => {{
          td.classList.toggle('match', td.textContent.toLowerCase().includes(q));
        }});
      }} else {{
        Array.from(tr.cells).forEach(td => td.classList.remove('match'));
      }}
    }});
  }}

  // debounce helper
  function debounce(fn, wait=120){{
    let t;
    return (...a) => {{ clearTimeout(t); t = setTimeout(()=>fn(...a), wait); }};
  }}

  searchInput.addEventListener('input', debounce(e => filterRows(e.target.value)));

  // optional: expose functions for external control
  window.__todoTable = {{
    sortBy,
    filterRows,
    readRows
  }};
}})();
</script>");
    }
}