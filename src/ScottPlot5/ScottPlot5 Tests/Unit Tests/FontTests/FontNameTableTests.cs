using ScottPlot.FontResolvers;
using System.Text;

namespace ScottPlotTests.FontTests;

internal class FontNameTableTests
{
    private record NameRecord(int Platform, int Encoding, int Language, int NameId, string Value);

    /// <summary>
    /// Build a minimal OpenType 'name' table (format 0) from the given records
    /// </summary>
    private static byte[] BuildNameTable(params NameRecord[] records)
    {
        List<byte> strings = [];
        List<byte> header = [];

        void U16(List<byte> list, int value)
        {
            list.Add((byte)(value >> 8));
            list.Add((byte)value);
        }

        U16(header, 0); // format
        U16(header, records.Length);
        U16(header, 6 + records.Length * 12); // string storage offset

        foreach (NameRecord r in records)
        {
            byte[] bytes = r.Platform is 0 or 3
                ? Encoding.BigEndianUnicode.GetBytes(r.Value)
                : Encoding.ASCII.GetBytes(r.Value);

            U16(header, r.Platform);
            U16(header, r.Encoding);
            U16(header, r.Language);
            U16(header, r.NameId);
            U16(header, bytes.Length);
            U16(header, strings.Count);
            strings.AddRange(bytes);
        }

        return [.. header, .. strings];
    }

    [Test]
    public void Test_NameTable_KeepsEveryLanguage()
    {
        // Same layout as Microsoft YaHei: English (0x409) then Simplified Chinese (0x804)
        byte[] table = BuildNameTable(
            new(3, 1, 0x409, 1, "Microsoft YaHei"),
            new(3, 1, 0x409, 4, "Microsoft YaHei Bold"),
            new(3, 1, 0x804, 1, "微软雅黑"),
            new(3, 1, 0x804, 4, "微软雅黑 Bold"));

        var names = FontNames.ParseNameTable(table);

        names[1].Should().BeEquivalentTo(["Microsoft YaHei", "微软雅黑"]);
        names[4].Should().BeEquivalentTo(["Microsoft YaHei Bold", "微软雅黑 Bold"]);
    }

    [Test]
    public void Test_NameTable_JapaneseAndKorean()
    {
        byte[] table = BuildNameTable(
            new(3, 1, 0x409, 1, "MS Gothic"),
            new(3, 1, 0x411, 1, "ＭＳ ゴシック"),
            new(3, 1, 0x409, 4, "Malgun Gothic"),
            new(3, 1, 0x412, 4, "맑은 고딕"));

        var names = FontNames.ParseNameTable(table);

        names[1].Should().BeEquivalentTo(["MS Gothic", "ＭＳ ゴシック"]);
        names[4].Should().BeEquivalentTo(["Malgun Gothic", "맑은 고딕"]);
    }

    [Test]
    public void Test_NameTable_DuplicateNamesAreCollapsed()
    {
        byte[] table = BuildNameTable(
            new(0, 3, 0, 1, "Same"),
            new(3, 1, 0x409, 1, "Same"));

        FontNames.ParseNameTable(table)[1].Should().Equal("Same");
    }

    [Test]
    public void Test_NameTable_MacRomanOnlyUsedAsFallback()
    {
        byte[] table = BuildNameTable(
            new(1, 0, 0, 1, "MacOnly"),
            new(1, 0, 0, 4, "MacFull"),
            new(3, 1, 0x409, 4, "WinFull"));

        var names = FontNames.ParseNameTable(table);

        names[1].Should().Equal("MacOnly"); // no Unicode record, so Mac is used
        names[4].Should().Equal("WinFull"); // Unicode record wins over Mac
    }

    [Test]
    public void Test_NameTable_NonRomanMacRecordsIgnored()
    {
        // Mac encoding 2 = Chinese Traditional: bytes are not Latin text and must not be decoded as such
        byte[] table = BuildNameTable(new NameRecord(1, 2, 0, 1, "garbage"));
        FontNames.ParseNameTable(table).Should().BeEmpty();
    }

    [Test]
    public void Test_NameTable_IgnoresUnrelatedNameIds()
    {
        byte[] table = BuildNameTable(
            new(3, 1, 0x409, 0, "Copyright"),
            new(3, 1, 0x409, 1, "Family"));

        FontNames.ParseNameTable(table).Keys.Should().Equal(1);
    }

    [Test]
    public void Test_NameTable_MalformedDataDoesNotThrow()
    {
        FontNames.ParseNameTable([]).Should().BeEmpty();
        FontNames.ParseNameTable([0, 0, 0, 50, 0, 6]).Should().BeEmpty(); // claims 50 records, has none

        // record points past the end of the data
        byte[] truncated = BuildNameTable(new NameRecord(3, 1, 0x409, 1, "Family"));
        FontNames.ParseNameTable(truncated[..^4]).Should().BeEmpty();
    }

    [Test]
    public void Test_Normalize_KeepsCjkCharacters()
    {
        FontNames.Normalize("微软雅黑 Bold").Should().Be("微软雅黑bold");
        FontNames.Normalize("ＭＳ ゴシック").Should().NotBeEmpty();
        FontNames.Normalize("Microsoft YaHei-Bold").Should().Be(FontNames.Normalize("microsoft yahei bold"));
    }
}
