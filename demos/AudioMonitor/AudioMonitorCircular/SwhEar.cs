using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioMonitor
{

    public class SwhEar
    {
        public int SAMPLERATE = 8000;
        int BITRATE = 16;
        int CHANNELS = 1;
        int BUFFERMILLISEC = 20;
        int STORESECONDS = 5;
        int bufferIndex = 0;
        public int buffersCaptured = 0;
        public int beatThreshold = 3500;
        public double signalMultiple = 1;

        NAudio.Wave.WaveInEvent wvin;

        public double[] values;
        public double[] times;

        public SwhEar(int deviceNumber)
        {
            Console.WriteLine($"Preparing audio device: {deviceNumber}");
            wvin = new NAudio.Wave.WaveInEvent();
            wvin.DeviceNumber = deviceNumber;
            wvin.WaveFormat = new NAudio.Wave.WaveFormat(SAMPLERATE, BITRATE, CHANNELS);
            wvin.BufferMilliseconds = BUFFERMILLISEC;
            wvin.DataAvailable += OnDataAvailable;
            Start();
        }

        public void Start()
        {
            Console.WriteLine($"Starting recording...");
            wvin.StartRecording();
        }

        public void Stop()
        {
            wvin.StopRecording();
            Console.WriteLine($"Recording stopped.");
        }

        public int lastPointUpdated = 0;
        public double lastAmplitude = 0;
        private void OnDataAvailable(object sender, NAudio.Wave.WaveInEventArgs args)
        {
            // convert from a 16-bit byte array to a double array
            int bytesPerValue = BITRATE / 8;
            int valuesInBuffer = args.BytesRecorded / bytesPerValue;
            double[] bufferValues = new double[valuesInBuffer];
            for (int i = 0; i < valuesInBuffer; i++)
                bufferValues[i] = BitConverter.ToInt16(args.Buffer, i * bytesPerValue) * signalMultiple;

            lastAmplitude = bufferValues.Max() - bufferValues.Min();

            // create the values buffer if it does not exist
            if (values == null)
            {
                int idealSampleCount = STORESECONDS * SAMPLERATE;
                int bufferCount = idealSampleCount / valuesInBuffer;
                values = new double[bufferCount * valuesInBuffer];
                times = new double[bufferCount * valuesInBuffer];
                for (int i = 0; i < times.Length; i++)
                    times[i] = (double)i / SAMPLERATE;
            }

            // copy these data into the correct place of the larger buffer
            Array.Copy(bufferValues, 0, values, bufferIndex * valuesInBuffer, bufferValues.Length);
            lastPointUpdated = bufferIndex * valuesInBuffer + bufferValues.Length;

            // update counts
            buffersCaptured += 1;
            bufferIndex += 1;
            if (bufferIndex * valuesInBuffer > values.Length - 1)
                bufferIndex = 0;
        }
    }
}
