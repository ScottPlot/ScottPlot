using System.Text;

namespace ScottPlot.FontResolvers;

/// <summary>
/// Helpers for finding the names a user might use to refer to a specific font face.
/// Windows may group a face into a different family than its file-level name suggests
/// (e.g., "Whitney Condensed Medium" is listed under the "Whitney" family),
/// so faces are also matched by their full, PostScript, and preferred names.
/// </summary>
public static class FontNames
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

        Dictionary<int, List<string>> nameTable = ReadNameTable(typeface);

        // Fonts often store the same name in several languages (e.g., "Microsoft YaHei" and "微软雅黑"),
        // so every localized name is indexed, not just one.
        foreach (int nameId in new[] { 1, 4, 6 })
        {
            if (nameTable.TryGetValue(nameId, out List<string>? values))
                foreach (string value in values)
                    Add(value);
        }

        if (nameTable.TryGetValue(16, out List<string>? preferredFamilies) &&
            nameTable.TryGetValue(17, out List<string>? preferredStyles))
        {
            foreach (string family in preferredFamilies)
                foreach (string style in preferredStyles)
                    Add($"{family} {style}");
        }

        return names;
    }

    /// <summary>
    /// Read the name records (name IDs 1, 4, 6, 16, 17) from a typeface's 'name' table.
    /// Returns an empty dictionary if the table cannot be read.
    /// </summary>
    private static Dictionary<int, List<string>> ReadNameTable(SKTypeface typeface)
    {
        try
        {
            uint tag = ('n' << 24) | ('a' << 16) | ('m' << 8) | 'e';
            byte[]? data = typeface.GetTableData(tag);
            return data is null ? [] : ParseNameTable(data);
        }
        catch
        {
            // Name lookup is a best-effort convenience, so never let it break rendering
            return [];
        }
    }

    /// <summary>
    /// Parse a raw OpenType 'name' table, returning every distinct name (in all languages)
    /// for name IDs 1, 4, 6, 16, and 17. Windows/Unicode records are preferred and
    /// Macintosh Roman records are only used for IDs that have no Unicode record.
    /// </summary>
    public static Dictionary<int, List<string>> ParseNameTable(byte[] data)
    {
        Dictionary<int, List<string>> unicodeNames = [];
        Dictionary<int, List<string>> macNames = [];

        if (data.Length < 6)
            return unicodeNames;

        int count = (data[2] << 8) | data[3];
        int stringOffset = (data[4] << 8) | data[5];

        for (int i = 0; i < count; i++)
        {
            int record = 6 + i * 12;
            if (record + 12 > data.Length)
                break;

            int platform = (data[record] << 8) | data[record + 1];
            int encoding = (data[record + 2] << 8) | data[record + 3];
            int nameId = (data[record + 6] << 8) | data[record + 7];
            int length = (data[record + 8] << 8) | data[record + 9];
            int offset = (data[record + 10] << 8) | data[record + 11];

            if (nameId is not (1 or 4 or 6 or 16 or 17))
                continue;

            int start = stringOffset + offset;
            if (start + length > data.Length)
                continue;

            bool unicode = platform is 0 or 3;
            bool macRoman = platform == 1 && encoding == 0; // other Mac encodings are not Latin text
            if (!unicode && !macRoman)
                continue;

            string name = unicode
                ? Encoding.BigEndianUnicode.GetString(data, start, length)
                : Encoding.GetEncoding("ISO-8859-1").GetString(data, start, length);

            Dictionary<int, List<string>> target = unicode ? unicodeNames : macNames;
            if (!target.TryGetValue(nameId, out List<string>? list))
                target[nameId] = list = [];
            if (!list.Contains(name))
                list.Add(name);
        }

        foreach (KeyValuePair<int, List<string>> mac in macNames)
        {
            if (!unicodeNames.ContainsKey(mac.Key))
                unicodeNames[mac.Key] = mac.Value;
        }

        return unicodeNames;
    }
}
