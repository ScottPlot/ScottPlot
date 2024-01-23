using ScottPlot;

Plot plt = new();
plt.Style.DarkMode();

Color[] colors = { Colors.Red, Colors.Green, Colors.Yellow, Colors.LightGray };

double x = 0;
for (int i = 0; i < 10; i++)
{
    double width = Random.Shared.Next(1, 5);
    x += width;
    double y = Random.Shared.Next(1, 3);

    CoordinateRect cr = new(x, x + width, y, y + 1);
    var rp = plt.Add.Rectangle(cr);
    rp.LineStyle.Width = 0;
    rp.FillStyle.Color = colors[Random.Shared.Next(colors.Length)];
}

plt.SavePng("test.png", 600, 300);
