namespace ScottPlot.Reporting;

public class Report
{
    public List<IComponent> ComponentList { get; } = [];

    public void AddHeading(string text, int level = 1)
    {
        ComponentList.Add(new Components.HeadingComponent(text, level));
    }

    public void AddPlot(Plot plot)
    {
        ComponentList.Add(new Components.PlotComponent(plot));
    }

    public void AddPlot(Plot plot, int width, int height)
    {
        ComponentList.Add(new Components.PlotComponent(plot)
        {
            Width = width,
            Height = height,
        });
    }

    public void AddPlot(Plot plot, string title, string description)
    {
        ComponentList.Add(new Components.PlotComponent(plot)
        {
            Title = title,
            Description = description,
        });
    }

    public void AddText(string text)
    {
        ComponentList.Add(new Components.TextComponent(text));
    }

    public void AddPageBreak()
    {
        ComponentList.Add(new Components.PageBreakComponent());
    }
}
