using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ScottPlotABF
{
    public class ABF
    {
        public string GetSWHProtocol(byte[] array, int stopLookingAfterByte = 50000)
        {
            int start = 0;
            int end = 0;
            for (int i = 0; i < stopLookingAfterByte; i++)
            {
                if (array[i] == '\\') start = i;
                if (array[i] == '.' && array[i + 1] == 'p' && array[i + 2] == 'r' && array[i + 3] == 'o')
                {
                    end = i;
                    break;
                } 
            }
            string protocol = "";
            for (int i = start+1; i < end; i++)
            {
                protocol += Convert.ToChar(array[i]);
            }
            return protocol;
        }

        // given a byte array, return the first position of non-zero data
        public int GetHeaderLength(byte[] array, int stopLookingAfterByte=50000)
        {
            stopLookingAfterByte = Math.Min(stopLookingAfterByte, array.Length - 1);
            int run = 0;
            int lastZero = 0;
            for (int i = 0; i < stopLookingAfterByte; i++)
            {
                if (array[i] == 0)
                {
                    run += 1;
                    if (run > 10) lastZero = i;
                }
                else
                {
                    run = 0;
                }
            }
            return lastZero+1;
        }
        public int GetTrailerLength(byte[] array, int stopLookingAfterByte = 50000)
        {
            stopLookingAfterByte = Math.Min(array.Length-stopLookingAfterByte, array.Length - 1);
            stopLookingAfterByte = Math.Max(stopLookingAfterByte,1);
            int run = 0;
            int lastZero = 0;
            int lastRun = 0;
            for (int i = array.Length - 1; i > stopLookingAfterByte; i--)
            {
                if (array[i] == 0)
                {
                    run += 1;
                    if (run > 10)
                    {
                        lastZero = i;
                        lastRun +=1;
                    }
                }
                else
                {
                    run = 0;
                    lastRun = 0;
                }
            }
            return array.Length - (lastZero - lastRun);
        }
    }
}
