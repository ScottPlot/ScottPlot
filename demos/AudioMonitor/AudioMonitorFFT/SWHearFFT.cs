using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioMonitor
{
    class SWHearFFT
    {
        public int SampleRate = 22050;
        public int BitRate = 16;
        int BufferMilliseconds = 47; // produces buffers of 1036 (close to 1024)
        int MicrophoneChannels = 1; // 1 for mono
        public int BuffersRead { get; private set; }
        public int DeviceIndex { get; private set; }
        bool IsRecording = false;
        public double[] fftData;
        WaveInEvent wvin;

        public SWHearFFT(int deviceIndex = 0)
        {
            DeviceIndex = deviceIndex;

            wvin = new WaveInEvent();
            wvin.DeviceNumber = DeviceIndex;
            wvin.WaveFormat = new NAudio.Wave.WaveFormat(SampleRate, BitRate, MicrophoneChannels);
            wvin.DataAvailable += OnDataAvailable;
            wvin.BufferMilliseconds = BufferMilliseconds;

            double pointsPerMs = (double)SampleRate / 1000;
            int bufferPoints = (int)(pointsPerMs * BufferMilliseconds);
            fftData = new double[bufferPoints / 2];
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

        private double[] FFT_from_PCM(double[] pcm)
        {

            // use the largest FFT size we can given the data (must be a power of 2)
            int fftPoints = 2;
            while (fftPoints * 2 <= pcm.Length)
                fftPoints *= 2;

            // prepare the complex data which will be FFT'd (using a window function)
            NAudio.Dsp.Complex[] fftFull = new NAudio.Dsp.Complex[fftPoints];
            for (int i = 0; i < fftPoints; i++)
                fftFull[i].X = (float)(pcm[i] * NAudio.Dsp.FastFourierTransform.HammingWindow(i, fftPoints));

            // perform the FFT
            NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(fftPoints, 2.0), fftFull);

            // average (sum) the mirror image frequency powers
            double[] fft = new double[fftPoints/2];
            for (int i = 0; i < fftPoints/2; i++)
            {
                double fftLeft = Math.Abs(fftFull[i].X + fftFull[i].Y);
                double fftRight = Math.Abs(fftFull[fftPoints-i-1].X + fftFull[fftPoints-i-1].Y);
                fft[i] = fftLeft+ fftRight;
            }

            return fft;
        }

        public double lastBufferAmplitude;
        private void OnDataAvailable(object sender, WaveInEventArgs args)
        {
            // convert byte array to Int16 array
            int bytesPerSample = BitRate / 8;
            int samplesRecorded = args.BytesRecorded / bytesPerSample;
            double[] pcm = new double[samplesRecorded];
            for (int i = 0; i < samplesRecorded; i++)
                pcm[i] = BitConverter.ToInt16(args.Buffer, i * bytesPerSample);

            // calculate amplitude
            lastBufferAmplitude = pcm.Max() - pcm.Min();

            // add this buffer to the circular buffer
            double maxAmplitude = Math.Pow(2, BitRate) / 2;

            // update the FFT
            FFT_from_PCM(pcm).CopyTo(fftData, 0);

            BuffersRead += 1;
        }
    }
}
