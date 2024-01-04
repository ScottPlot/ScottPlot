ScottPlot.Plot plt = new();

double[] sales = { 123, 456, 789 };
double[] salesError = { 50, 150, 350 };
DateTime[] dates = { new(2024, 01, 01), new(2024, 01, 02), new(2024, 01, 03) };
double[] dateXs = dates.Select(x => x.ToOADate()).ToArray();

plt.Add.Scatter(dateXs, sales);
plt.Add.ErrorBar(dateXs, sales, salesError);

plt.SavePng("test.png", 400, 300);
