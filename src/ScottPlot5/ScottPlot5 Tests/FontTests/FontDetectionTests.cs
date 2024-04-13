using System.Diagnostics;

namespace ScottPlotTests.FontTests;

internal class FontDetectionTests
{
    static void ConvertStringToTextElements_UnitTests()
    {
        List<string> testcases = ["𝓦", "á", "🌹", "👩🏽‍🚒", "已", "a", "𝓦á🌹👩🏽‍🚒已"];
        List<List<string>> expectedResults =
            [["𝓦"], ["á"], ["🌹"], ["👩🏽‍🚒"], ["已"], ["a"], ["𝓦", "á", "🌹", "👩🏽‍🚒", "已"]];

        foreach (var (testcase, expected) in testcases.Zip(expectedResults))
        {
            var res = Fonts.ConvertStringToTextElements(testcase);

            foreach (var (resVal, expVal) in res.Zip(expected))
                if (resVal != expVal)
                    throw new InvalidOperationException($"ConvertStringToTextElementList_UnitTests() error for '{testcase}'");

            var resStr = string.Join(", ", res.Select(x => $"'{x}'"));
            Debug.WriteLine($"Test: Converted '{testcase}' to text elements = {resStr}");
        }
    }

    static void GetStandaloneCodePoints_UnitTests()
    {
        List<string> testcases = ["𝓦", "á", "🌹", "👩🏽‍🚒", "已", "a", "𝓦á"];
        List<List<string>> expectedResults = [["𝓦"], [], ["🌹"], [], ["已"], ["a"], ["𝓦"]];

        foreach (var testcase in testcases)
        {
            var testcaseTextElements = Fonts.ConvertStringToTextElements(testcase);
            var testcaseCodePoints = testcaseTextElements.Select(Fonts.ConvertTextElementToUtf32CodePoints).ToList();
            var res = Fonts.GetStandaloneCodePoints(testcaseCodePoints);

            // TODO: Add automatic checking, was done manually 

            var inpCodePointsStr =
                string.Join(", ",
                    testcaseCodePoints.Select(x => "[" + string.Join(", ", x.Select(y => $"0x{y:X08}")) + "]"));
            var resCodePointsStr = string.Join(", ", res.Select(x => $"0x{x:X08}"));
            Debug.WriteLine($"Test: Input string '{testcase}' has these standalone code points = {resCodePointsStr} from {inpCodePointsStr}");
        }
    }

    [Test]
    public static void ConvertTextElementToUtf32CodePoints_UnitTests()
    {
        List<string> testcases = ["𝓦", "á", "🌹", "👩🏽‍🚒", "已", "a"];

        foreach (var testcase in testcases)
        {
            var res = Fonts.ConvertTextElementToUtf32CodePoints(testcase);
            var res_rev = string.Join("", res.Select(char.ConvertFromUtf32));

            var cmp = res_rev == testcase;

            if (!cmp)
                throw new InvalidOperationException($"ConvertTextElementToUtf32CodePoints_UnitTests() error for '{testcase}'");

            var codePointsStr = string.Join(", ", res.Select(x => $"0x{x:X08}"));
            Debug.WriteLine($"Test: Converted '{testcase}' to Utf32 code points = {codePointsStr}");
        }
    }
}
