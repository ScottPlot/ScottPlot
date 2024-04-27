namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    System.Windows.Forms.Timer Timer = new() { Interval = 10 };

    public Form1()
    {
        InitializeComponent();

        var logger = formsPlot1.Plot.Add.DataLogger();
        logger.Add(0, 0);
        logger.LineWidth = 2;

        double xMult = 1;
        double yMult = 1;

        Timer.Tick += (s, e) =>
        {
            if (Random.Shared.Next(100) == 1) xMult = -xMult;
            if (Random.Shared.Next(100) == 1) yMult = -yMult;

            double x = logger.Data.Coordinates.Last().X + Random.Shared.NextDouble() * xMult;
            double y = logger.Data.Coordinates.Last().Y + Random.Shared.NextDouble() * yMult;
            logger.Add(x, y);

            formsPlot1.Refresh();
        };

        Timer.Start();

    }
}
