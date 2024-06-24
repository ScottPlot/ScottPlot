var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async context =>
{
    string html = "<html><body><img src='random.png'></body></html>";
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(html);
});

app.MapGet("/random.png", async context =>
{
    // create a plot and fill it with sample data
    ScottPlot.Plot myPlot = new();
    double[] dataX = ScottPlot.Generate.Consecutive(100);
    double[] dataY = ScottPlot.Generate.RandomWalk(100);
    myPlot.Add.Scatter(dataX, dataY);

    // render the plot as an image and serve it
    byte[] imageBytes = myPlot.GetImageBytes(600, 400, ScottPlot.ImageFormat.Png);
    context.Response.ContentType = "image/png";
    await context.Response.Body.WriteAsync(imageBytes, 0, imageBytes.Length);
});

app.MapGet("/svg", async context =>
{
    // create a plot and fill it with sample data
    ScottPlot.Plot myPlot = new();
    double[] dataX = ScottPlot.Generate.Consecutive(100);
    double[] dataY = ScottPlot.Generate.RandomWalk(100);
    myPlot.Add.Scatter(dataX, dataY);

    // render the plot as a SVG string and serve it inside HTML
    string svg = myPlot.GetSvgXml(600, 400);
    string html = $"<html><body>{svg}</body></html>";
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(html);
});

app.Run();
