using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magisterka
{
    abstract class Parser
    {
        public int magicNumber;
        public int sampleSize = 1;
        public int samplesCount;
        public long offSet;
        public long headerOffSet;
        public string path;
        public int currentSampleId = 0;

        abstract public void ReadHeader();

        public double[] ByteArrayToDoubleArray(byte[] digit)
        {
            double[] doubleArray = new double[sampleSize];
            for (int i = 0; i < sampleSize; i++)
            {
                doubleArray[i] = digit[i];
            }
            return doubleArray;
        }

        public byte[] ReadNext()
        {
            using (FileStream stream = File.Open(path, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                if (currentSampleId < samplesCount)
                {
                    return ReadNextSymbol(reader);
                }
                else
                {
                    currentSampleId = 0;
                    offSet = headerOffSet;
                    return ReadNextSymbol(reader);
                }
            }
        }

        protected byte[] ReadNextSymbol(BinaryReader reader)
        {
            reader.BaseStream.Seek(offSet, SeekOrigin.Begin);
            byte[] digit = new byte[sampleSize];

            for (int i = 0; i < sampleSize; i++)
            {
                digit[i] = reader.ReadByte();
            }
            offSet = reader.BaseStream.Position;
            currentSampleId++;
            return digit;
        }

        public int GetMagicNumber()
        {
            return magicNumber;
        }

        public int GetSampleCount()
        {
            return samplesCount;
        }

        public long GetOffSet()
        {
            return offSet;
        }

        protected int SwapEndianness(int value)
        {
            var b1 = (value >> 0) & 0xff;
            var b2 = (value >> 8) & 0xff;
            var b3 = (value >> 16) & 0xff;
            var b4 = (value >> 24) & 0xff;

            return b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0;
        }
    }
}
