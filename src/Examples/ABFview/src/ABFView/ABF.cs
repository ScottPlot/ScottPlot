using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace ABFView
{
    class ABF
    {

        public string abfFilePath;
        public string abfID;
        public ABF(string filename)
        {
            abfFilePath = System.IO.Path.GetFullPath(filename);
            abfID = System.IO.Path.GetFileNameWithoutExtension(abfFilePath);
            if (!System.IO.File.Exists(abfFilePath))
                throw new ArgumentException($"file does not exist:\n{abfFilePath}");
            Load();
        }
        
        public int sweepCount;
        public int sweepPointCount;

        //public double[] data;
        public List<double> data;
        
        public void Load()
        {
            // read header info
            BinaryReader fb = new BinaryReader(File.Open(abfFilePath, FileMode.Open));
            if (new string(fb.ReadChars(4)) != "ABF2")
                throw new System.ArgumentException($"Not a valid ABF2 file:\n{abfFilePath}");
            int BLOCKSIZE = 512; // blocks are a fixed size in ABF1 and ABF2 files
            fb.BaseStream.Seek(12, SeekOrigin.Begin); // this byte stores the number of sweeps
            sweepCount = (int)fb.ReadUInt32();
            fb.BaseStream.Seek(76, SeekOrigin.Begin); // this byte stores the ProtocolSection block number
            long posProtocolSection = fb.ReadUInt32() * BLOCKSIZE;
            long poslADCResolution = posProtocolSection + 118; // figure out where lADCResolution lives
            fb.BaseStream.Seek(poslADCResolution, SeekOrigin.Begin); // then go there
            long lADCResolution = fb.ReadInt32();
            float dataScaleFactor = lADCResolution / (float)1e6;
            fb.BaseStream.Seek(236, SeekOrigin.Begin); // this byte stores the DataSection block number
            long dataFirstByte = fb.ReadUInt32() * BLOCKSIZE;
            long dataPointByteSize = fb.ReadUInt32(); // this will always be 2 for a 16-bit DAC
            long dataPointCount = fb.ReadInt64();
            sweepPointCount = (int)(dataPointCount / sweepCount);

            // load all data into memory
            int dataByteCount = (int)(dataPointCount * dataPointByteSize);
            byte[] dataBytes = new byte[dataByteCount];
            short[] dataRaw = new short[dataPointCount];
            double[] dataValues = new double[dataPointCount];
            fb.BaseStream.Seek(dataFirstByte, SeekOrigin.Begin);
            fb.BaseStream.Read(dataBytes, 0, dataByteCount);
            Buffer.BlockCopy(dataBytes, 0, dataRaw, 0, dataByteCount);
            for (long i = 0; i < dataPointCount; i++) dataValues[i] = dataRaw[i] * dataScaleFactor;
            data = dataValues.ToList();

            // close the file ASAP
            fb.Close();
        }

    }
}
