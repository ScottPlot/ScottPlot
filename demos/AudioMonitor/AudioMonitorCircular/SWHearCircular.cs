using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioMonitor
{
    class SWHearCircular
    {
        public int SampleRate = 44100;
        public int BitRate = 16;
        int BufferMilliseconds = 20; // must divide evenly into (SampleRate)
        int MicrophoneChannels = 1; // 1 for mono
        public int BuffersRead { get; private set; }
        public int DeviceIndex { get; private set; }
        bool IsRecording = false;
        WaveInEvent wvin;

        int CircularBufferSizeSec = 3;
        public double[] CircularBuffer;
        int CircularBufferNextIndex = 0;
        public double CircularBufferNextIndexSec { get { return (double)CircularBufferNextIndex / SampleRate; } }

        public SWHearCircular(int deviceIndex = 0)
        {
            DeviceIndex = deviceIndex;

            wvin = new WaveInEvent();
            wvin.DeviceNumber = DeviceIndex;
            wvin.WaveFormat = new NAudio.Wave.WaveFormat(SampleRate, BitRate, MicrophoneChannels);
            wvin.DataAvailable += OnDataAvailable;
            wvin.BufferMilliseconds = BufferMilliseconds;

            CircularBuffer = new double[CircularBufferSizeSec * SampleRate];
        }

        public void Open()
        {
            if (!IsRecording)
            {
                IsRecording = true;
                wvin.StartRecording();
            }
        }

        public void Close()
        {
            if (IsRecording)
            {
                wvin.StopRecording();
                IsRecording = false;
            }
        }

        public int lastBufferAmplitude;
        private void OnDataAvailable(object sender, WaveInEventArgs args)
        {
            // convert byte array to Int16 array
            int bytesPerSample = BitRate / 8;
            int samplesRecorded = args.BytesRecorded / bytesPerSample;
            Int16[] lastBuffer = new Int16[samplesRecorded];
            for (int i = 0; i < samplesRecorded; i++)
                lastBuffer[i] = BitConverter.ToInt16(args.Buffer, i * bytesPerSample);

            // calculate amplitude
            lastBufferAmplitude = lastBuffer.Max() - lastBuffer.Min();

            // add this buffer to the circular buffer
            double maxAmplitude = Math.Pow(2, BitRate) / 2;
            for (int i = 0; i < lastBuffer.Length; i++)
                CircularBuffer[CircularBufferNextIndex + i] = (double)lastBuffer[i] / maxAmplitude;
            CircularBufferNextIndex += lastBuffer.Length;
            if (CircularBufferNextIndex == CircularBuffer.Length)
                CircularBufferNextIndex = 0;

            BuffersRead += 1;
        }
    }
}
