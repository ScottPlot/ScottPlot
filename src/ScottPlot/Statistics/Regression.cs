using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Statistics
{
	public interface IRegressionLine {
		double[] GetCoefficients();
		double[] Residual();
		Plot Draw(Plot plot);
	}

	public class LinearRegressionLine : IRegressionLine {
		private double[] x;
		private double[] y;
		private double[] coefficients;

		public LinearRegressionLine(double[] xAxis, double[] yAxis) {
			if (xAxis.Length != yAxis.Length) {
				throw new ArgumentException("Axes of mismatched length");
			}

			x = xAxis;
			y = yAxis;
			coefficients = GetCoefficients();
		}

		private double SetMean(double[] set) {
			double total = 0;

			foreach (double curr in set) {
				total += curr;
			}

			return total / set.Length;
		}

		private double y_hat(double x) {//Value predicted by regression model
			//double[] coefficients = GetCoefficients();

			return coefficients[0] + coefficients[1] * x;
		}

		public double[] GetCoefficients() {//y_hat = a+bx
			double a;
			double b;

			double xMean=SetMean(x);
			double yMean=SetMean(y);

			double sumXYResidual = 0;
			double sumXSquareResidual = 0;

			for (int i = 0; i < x.Length; i++)
			{
				sumXYResidual += (x[i] - xMean) * (y[i] - yMean);
				sumXSquareResidual += Math.Pow((x[i] - xMean), 2);
			}

			b = sumXYResidual / (sumXSquareResidual);

			a = yMean - (b * xMean);//LSRL always passes through the point (x̅,y̅)

			return new double[] { a, b };
		}

		public double[] Residual() {
			//double[] coefficients = GetCoefficients();

			double[] residuals = new double[y.Length];

			for (int i = 0; i < y.Length; i++) {
				residuals[i] = y[i] - y_hat(x[i]);
			}

			return residuals;
		}

		public Plot Draw(Plot plot) {
			double[] xPoints = new double[2];
			double[] yPoints = new double[2];

			xPoints[0] = x[0];
			yPoints[0] = y_hat(xPoints[0]);
			xPoints[1] = x[x.Length - 1];
			yPoints[1] = y_hat(xPoints[1]);

			double middleX = (xPoints[0] + xPoints[1]) / 2.0;

			plot.PlotScatter(xPoints, yPoints, markerSize: 0, lineStyle: LineStyle.Dash);
			plot.PlotText($"ŷ = {coefficients[0]} + {coefficients[1]}x", middleX, yPoints[1], alignment: TextAlignment.middleCenter);

			return plot;
		}

		public Plot DrawResidual(Plot plot) {
			double[] residuals = Residual();

			plot.PlotScatter(x, residuals, lineWidth: 0);

			return plot;
		}
	}
}
