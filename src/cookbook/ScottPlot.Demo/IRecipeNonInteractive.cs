namespace ScottPlot.Demo
{
    /// <summary>
    /// Implement in recipes which create bitmaps but cannoy be displayed in user controls
    /// </summary>
    public interface IRecipeNonInteractive
    {
        System.Drawing.Bitmap Render(int width, int height);
    }
}
