using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Statistics
{
	public interface IRegressionLine {
		double[] GetCoefficients();
		double[] Residual();
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

		private double y_hat(double x) {//Value predicted by regression model
			//double[] coefficients = GetCoefficients();

			return coefficients[0] + coefficients[1] * x;
		}

		public double[] GetCoefficients() {//y_hat = a+bx
			double a;
			double b;

			double xMean=x.Average();
			double yMean=y.Average();

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

		public Plot DrawResidual(Plot plot) {
			double[] residuals = Residual();

			plot.PlotScatter(x, residuals, lineWidth: 0);

			return plot;
		}
	}
}
