using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magisterka
{
    class LabelParser : Parser
    {
        public LabelParser(string _path)
        {
            path = _path;
        }

        override public void ReadHeader()
        {
            using (FileStream stream = File.Open(path, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                magicNumber = SwapEndianness(reader.ReadInt32());
                samplesCount = SwapEndianness(reader.ReadInt32());
                offSet = reader.BaseStream.Position;
                headerOffSet = reader.BaseStream.Position;
            }
        }

        public byte GetLabel(byte[] byteArray)
        {
            if (byteArray.Length == 1)
                return byteArray[0];
            else
                throw (new ByteArrayToByteException("Byte array size is not 1"));
        }

        public double[] GetOutputFromByte(byte label)
        {
            int[] intArray = Enumerable.Repeat(0, 10).ToArray();
            double[] result = intArray.Select(x => (double)x).ToArray();
            result[label] = 1;
            return result;
        }
    }
}
