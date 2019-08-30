using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serial_data_plotter
{
    class AdcValuesLoop
    {
        public readonly double[] values1;
        public readonly double[] values2;
        public readonly double[] values3;
        public readonly double[] values4;
        public int nextIndex = 0;

        public AdcValuesLoop(int valueCount = 500)
        {
            values1 = new double[valueCount];
            values2 = new double[valueCount];
            values3 = new double[valueCount];
            values4 = new double[valueCount];
        }

        public void Add(double[] values)
        {
            if (nextIndex >= values1.Length)
                nextIndex = 0;

            values1[nextIndex] = values[0];
            values2[nextIndex] = values[1];
            values3[nextIndex] = values[2];
            values4[nextIndex] = values[3];

            nextIndex += 1;
        }

        public void Clear()
        {
            for (int i = 0; i < values1.Length; i++)
            {
                values1[i] = 0;
                values2[i] = 0;
                values3[i] = 0;
                values4[i] = 0;
            }
            nextIndex = 0;
        }

        public void ParseCsvLine(string line)
        {
            string[] parts = line.Split(',');
            double[] values = new double[parts.Length];
            for (int i = 0; i < parts.Length; i++)
                double.TryParse(parts[i], out values[i]);

            for (int i = 0; i < parts.Length; i++)
                if (values[i] > 64000)
                    values[i] = 0;

            Add(values);
        }
    }
}
