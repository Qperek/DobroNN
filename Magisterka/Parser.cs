using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magisterka
{
    abstract class Parser
    {
        public int magicNumber;
        public int sampleSize;
        public int samplesCount;
        public long offSet;
        public long headerOffSet;
        public string path;
        public int currentSampleId = 0;

        abstract public void ReadHeader();
        abstract public byte[] ReadNext();

        public double[] ByteArrayToDoubleArray(byte[] digit)
        {
            double[] doubleArray = new double[sampleSize];
            for (int i = 0; i < sampleSize; i++)
            {
                doubleArray[i] = digit[i];
            }
            return doubleArray;
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
