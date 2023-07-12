using SkiaSharp;

ScottPlot.Plot plot = new();

plot.Add.Signal(ScottPlot.Generate.Sin());
plot.Add.Signal(ScottPlot.Generate.Cos());

plot.TopAxis.Label.Text = "频率信号削减1/32";

Console.WriteLine(SKFontManager.Default.MatchCharacter('频').FamilyName);
plot.TopAxis.Label.Font.Name = SKFontManager.Default.MatchCharacter('频').FamilyName;

plot.SavePng("test.png", 400, 300);