var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => GetSampleImage());

app.Run();


string GetSampleImage() {


    ScottPlot.Plot myPlot = new();

    double[] dataX = { 1, 2, 3, 4, 5 };
    double[] dataY = { 1, 4, 9, 16, 25 };
    myPlot.Add.Scatter(dataX, dataY);


    var imgBytes =  myPlot.GetImageBytes(400,300, ScottPlot.ImageFormat.Png);

    var base64String = Convert.ToBase64String(imgBytes);
    //return base64String;

    //If trying to create a data URI, you can do something like this:
    var dataUri =  $"data:image/png;base64,{base64String}";

    //you can use this in an img tag in html like this:
    // <img src="data:image/png;base64,base64String" alt="ScottPlot Image" />        
    return dataUri;
}
