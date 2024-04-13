namespace ScottPlotTests.RenderTests.Plottable
{
    internal class AnnotationTests
    {
        [Test]
        public void Test_Annotation_Alignment()
        {
            ScottPlot.Plot plt = new();

            foreach (Alignment alignment in Enum.GetValues(typeof(Alignment)))
            {
                plt.Add.Annotation(alignment.ToString(), alignment);
            }

            plt.SaveTestImage();
        }
    }
}
