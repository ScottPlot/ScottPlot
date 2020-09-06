﻿/*
 *  Work in this file is derived from code originally written by Hans-Peter Moser:
 *  http://www.mosismath.com/Basics/Basics.html
 *  http://www.mosismath.com/Matrix_Gauss/MatrixGauss.html
 *  It is included in ScottPlot under a MIT license with permission from the original author.
 */

namespace ScottPlot.Statistics.Interpolation
{
    public class Matrix
    {
        public double[,] a;
        public double[] y;
        public double[] x;

        public Matrix(int size)
        {
            a = new double[size, size];
            y = new double[size];
            x = new double[size];
        }
    }
}
