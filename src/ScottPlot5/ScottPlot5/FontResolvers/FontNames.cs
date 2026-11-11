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
            {
                sb.Append(char.ToLowerInvariant(c));
            }
            else if (char.IsSurrogate(c) || IsCombiningMark(c))
            {
                // Keep characters outside the basic multilingual plane (e.g., rare CJK ideographs)
                // and the vowel signs and accents that are part of names in scripts like Thai and Devanagari
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    private static bool IsCombiningMark(char c)
    {
        return CharUnicodeInfo.GetUnicodeCategory(c)
            is UnicodeCategory.NonSpacingMark
            or UnicodeCategory.SpacingCombiningMark
            or UnicodeCategory.EnclosingMark;
    }

    /// <summary>
    /// Return every normalized name that can be used to refer to this face
    /// (the union of <see cref="GetFamilyNames(SKTypeface)"/> and <see cref="GetFaceNames(SKTypeface)"/>)
    /// </summary>
    public static HashSet<string> GetNames(SKTypeface typeface)
    {
        Dictionary<int, List<string>> nameTable = ReadNameTable(typeface);
        HashSet<string> names = GetFamilyNames(typeface, nameTable);
        names.UnionWith(GetFaceNames(nameTable));
        return names;
    }

    /// <summary>
    /// Return the normalized names of the families this face belongs to.
    /// These names are typically shared with the other faces of the family (bold, italic, etc.)
    /// </summary>
    public static HashSet<string> GetFamilyNames(SKTypeface typeface)
    {
        return GetFamilyNames(typeface, ReadNameTable(typeface));
    }

    /// <summary>
    /// Return the normalized names intended to identify this face alone
    /// (full name, PostScript name, and family names combined with their style names)
    /// </summary>
    public static HashSet<string> GetFaceNames(SKTypeface typeface)
    {
        return GetFaceNames(ReadNameTable(typeface));
    }

    private static void AddNormalized(HashSet<string> names, string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return;
        string normalized = Normalize(name!);
        if (normalized.Length > 0)
            names.Add(normalized);
    }

    private static HashSet<string> GetFamilyNames(SKTypeface typeface, Dictionary<int, List<string>> nameTable)
    {
        HashSet<string> names = [];

        AddNormalized(names, typeface.FamilyName);

        // Fonts often store the same name in several languages (e.g., "Microsoft YaHei" and "微软雅黑"),
        // so every localized name is indexed, not just one.
        // 1 = family, 16 = typographic family, 21 = WWS family
        foreach (int nameId in new[] { 1, 16, 21 })
        {
            if (nameTable.TryGetValue(nameId, out List<string>? values))
                foreach (string value in values)
                    AddNormalized(names, value);
        }

        return names;
    }

    private static HashSet<string> GetFaceNames(Dictionary<int, List<string>> nameTable)
    {
        HashSet<string> names = [];

        // 4 = full name, 6 = PostScript name
        foreach (int nameId in new[] { 4, 6 })
        {
            if (nameTable.TryGetValue(nameId, out List<string>? values))
                foreach (string value in values)
                    AddNormalized(names, value);
        }

        // family + style pairs: legacy (1, 2), typographic (16, 17), and WWS (21, 22)
        foreach ((int familyId, int styleId) in new[] { (1, 2), (16, 17), (21, 22) })
        {
            if (nameTable.TryGetValue(familyId, out List<string>? families) &&
                nameTable.TryGetValue(styleId, out List<string>? styles))
            {
                foreach (string family in families)
                    foreach (string style in styles)
                        AddNormalized(names, $"{family} {style}");
            }
        }

        return names;
    }

    /// <summary>
    /// Read the name records from a typeface's 'name' table.
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
    /// for name IDs 1, 2, 4, 6, 16, 17, 21, and 22. Windows/Unicode records are preferred and
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

            if (nameId is not (1 or 2 or 4 or 6 or 16 or 17 or 21 or 22))
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
                : DecodeMacRoman(data, start, length);

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

    /// <summary>
    /// Characters for Mac OS Roman bytes 0x80 through 0xFF (the lower half is ASCII).
    /// A table is used because this code page is not available on every .NET platform.
    /// </summary>
    private const string MacRomanUpperHalf =
        "ÄÅÇÉÑÖÜáàâäãåçéè" +
        "êëíìîïñóòôöõúùûü" +
        "†°¢£§•¶ß®©™´¨≠ÆØ" +
        "∞±≤≥¥µ∂∑∏π∫ªºΩæø" +
        "¿¡¬√ƒ≈∆«»… ÀÃÕŒœ" +
        "–—“”‘’÷◊ÿŸ⁄€‹›ﬁﬂ" +
        "‡·‚„‰ÂÊÁËÈÍÎÏÌÓÔ" +
        "ÒÚÛÙıˆ˜¯˘˙˚¸˝˛ˇ";

    private static string DecodeMacRoman(byte[] data, int start, int length)
    {
        StringBuilder sb = new(length);
        for (int i = start; i < start + length; i++)
        {
            byte b = data[i];
            sb.Append(b < 0x80 ? (char)b : MacRomanUpperHalf[b - 0x80]);
        }
        return sb.ToString();
    }
}
