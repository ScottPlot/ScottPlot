/* A 25-color pelette based on Anton Tsitsulin's 12-color palette discussed:
 * http://tsitsul.in/blog/coloropt/
 * At the time the license file was accessed (2021-09-03) in the github address:
 * https://github.com/xgfs/coloropt
 * the original work was released under a MIT License.
 */
namespace ScottPlot.Drawing.Colorsets
{
    class xgfs25 : HexColorset, IPalette
    {
        public override string[] hexColors => new string[]
        {
            "#ebac23", "#b80058", "#008cf9", "#006e00", "#00bbad",
            "#d163e6", "#b24502", "#ff9287", "#5954d6", "#00c6f8",
            "#878500", "#00a76c",
            "#f6da9c", "#ff5caa", "#8accff", "#4bff4b", "#6efff4",
            "#edc1f5", "#feae7c", "#ffc8c3", "#bdbbef", "#bdf2ff",
            "#fffc43", "#65ffc8",
            "#aaaaaa",
        };
    }
}
