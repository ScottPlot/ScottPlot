using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Diagnostics;

namespace ScottPlot
{
    public class DataGen
    {
        public Random rand = new Random(0);
        private Stopwatch stopwatch = Stopwatch.StartNew();

        /// <summary>
        /// ascending sequence of evenly spaced numbers
        /// </summary>
        public double[] Sequence(int count, double spacing = 1, double offset = 0)
        {
            double[] vals = new double[count];
            for (int i=0; i<count; i++)
            {
                vals[i] = i * spacing + offset;
            }
            return vals;
        }

        /// <summary>
        /// ascending sequence of randomly-spaced X data
        /// </summary>
        public double[] SequenceUneven(int count, double spacing = 1, double offset = 0)
        {
            double[] vals = new double[count];
            double sum = 0;
            for (int i = 0; i < count; i++)
            {
                vals[i] = sum;
                sum += rand.NextDouble();
            }
            return vals;
        }

        /// <summary>
        /// the same value over and over
        /// </summary>
        public double[] SingleValue(int count, double value)
        {
            double[] vals = new double[count];
            for (int i = 0; i < count; i++)
            {
                vals[i] = value;
            }
            return vals;
        }

        /// <summary>
        /// Return the Sine(X) given an array of Xs
        /// </summary>
        public double[] Sine(double[] Xs)
        {
            double[] vals = new double[Xs.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                vals[i] = Math.Sin(Xs[i]);
            }
            return vals;
        }

        public double[] SineAnimated(int pointCount = 1000)
        {
            double offset = stopwatch.ElapsedMilliseconds / 10;
            double[] vals = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
                vals[i] = Math.Sin(((double)i + offset) / 20);
            return vals;
        }

        /// <summary>
        /// return random numbers -0.5 to +0.5 times a multiplier and added to an offset
        /// </summary>
        /// <param name="count"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public double[] RandomValues(int count, double mult=1, double offset=0)
        {
            double[] vals = new double[count];
            for (int i = 0; i<count; i++)
            {
                vals[i] = rand.NextDouble() * mult + offset;
            }
            return vals;
        }

        /// <summary>
        /// integrated white noise
        /// </summary>
        public double[] RandomWalk(int count, double mult = 1, double offset = 0, bool startRandom=false)
        {
            double[] vals = new double[count];
            double runningSum=0;

            if (startRandom) runningSum+=rand.NextDouble() * count / 1000;

            for (int i = 0; i < count; i++)
            {
                runningSum += rand.NextDouble()-.5;
                vals[i] = runningSum * mult + offset;
            }
            
            return vals;
        }


        /// <summary>
        /// Return the given array with white noise added
        /// </summary>
        public double[] AddNoise(double[] vals, double scale=1, bool integrated = true)
        {
            // generate white noise
            double[] noiseVals = new double[vals.Length];
            for (int i = 0; i < noiseVals.Length; i++) noiseVals[i] += rand.NextDouble()-.5;

            // integreate the noise to create a random walk
            if (integrated)
            {
                double runningSum = 0;
                for (int i = 0; i < vals.Length; i++)
                {
                    // each point is pulled 75% to the last point and 25% to a new random one
                    runningSum = (runningSum*3 + noiseVals[i]) / 4.0;
                    noiseVals[i] = runningSum;
                }
            }

            // add the noise to the data
            for (int i = 0; i < vals.Length; i++) vals[i] += noiseVals[i] * scale;
            return vals;
        }

        public Color RandomColor(double alpha=1)
        {
            alpha = Math.Max(0, Math.Min(alpha, 255));
            return Color.FromArgb((int)alpha, rand.Next(256), rand.Next(256), rand.Next(256));
        }

        public Color randomColor {
            get
            {
                return Color.FromArgb(255, rand.Next(256), rand.Next(256), rand.Next(256));
            }
        }


    }
}
