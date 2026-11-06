using System.Text;

namespace ScottPlot.FontResolvers;

/// <summary>
/// Helpers for finding the names a user might use to refer to a specific font face.
/// Windows may group a face into a different family than its file-level name suggests
/// (e.g., "Whitney Condensed Medium" is listed under the "Whitney" family),
/// so faces are also matched by their full, PostScript, and preferred names.
/// </summary>
internal static class FontNames
{
    /// <summary>
    /// Make names comparable by ignoring case, spaces, hyphens, and underscores
    /// </summary>
    public static string Normalize(string name)
    {
        StringBuilder sb = new(name.Length);
        foreach (char c in name)
        {
            if (char.IsLetterOrDigit(c))
                sb.Append(char.ToLowerInvariant(c));
        }
        return sb.ToString();
    }

    /// <summary>
    /// Return every name that can be used to refer to this face
    /// </summary>
    public static HashSet<string> GetNames(SKTypeface typeface, string? familyName = null, string? styleName = null)
    {
        HashSet<string> names = [];

        void Add(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;
            string normalized = Normalize(name);
            if (normalized.Length > 0)
                names.Add(normalized);
        }

        if (!string.IsNullOrEmpty(familyName) && !string.IsNullOrEmpty(styleName))
            Add($"{familyName} {styleName}");

        Add(typeface.FamilyName);

        Dictionary<int, string> nameTable = ReadNameTable(typeface);
        nameTable.TryGetValue(1, out string? legacyFamily);
        nameTable.TryGetValue(4, out string? fullName);
        nameTable.TryGetValue(6, out string? postScriptName);
        nameTable.TryGetValue(16, out string? preferredFamily);
        nameTable.TryGetValue(17, out string? preferredStyle);

        Add(legacyFamily);
        Add(fullName);
        Add(postScriptName);
        if (preferredFamily is not null && preferredStyle is not null)
            Add($"{preferredFamily} {preferredStyle}");

        return names;
    }

    /// <summary>
    /// Read the English name records (name IDs 1, 4, 6, 16, 17) from a typeface's 'name' table.
    /// Returns an empty dictionary if the table cannot be read.
    /// </summary>
    private static Dictionary<int, string> ReadNameTable(SKTypeface typeface)
    {
        Dictionary<int, string> result = [];

        try
        {
            uint tag = ('n' << 24) | ('a' << 16) | ('m' << 8) | 'e';
            byte[]? data = typeface.GetTableData(tag);
            if (data is null || data.Length < 6)
                return result;

            int count = (data[2] << 8) | data[3];
            int stringOffset = (data[4] << 8) | data[5];

            for (int i = 0; i < count; i++)
            {
                int record = 6 + i * 12;
                if (record + 12 > data.Length)
                    break;

                int platform = (data[record] << 8) | data[record + 1];
                int nameId = (data[record + 6] << 8) | data[record + 7];
                int length = (data[record + 8] << 8) | data[record + 9];
                int offset = (data[record + 10] << 8) | data[record + 11];

                if (nameId is not (1 or 4 or 6 or 16 or 17))
                    continue;

                int start = stringOffset + offset;
                if (start + length > data.Length)
                    continue;

                // Prefer Windows/Unicode records, but accept Macintosh Roman if they are the only ones
                bool unicode = platform is 0 or 3;
                if (!unicode && platform != 1)
                    continue;
                if (!unicode && result.ContainsKey(nameId))
                    continue;

                result[nameId] = unicode
                    ? Encoding.BigEndianUnicode.GetString(data, start, length)
                    : Encoding.Latin1.GetString(data, start, length);
            }
        }
        catch
        {
            // Name lookup is a best-effort convenience, so never let it break rendering
        }

        return result;
    }
}
