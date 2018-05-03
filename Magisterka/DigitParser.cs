using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Magisterka
{
    class DigitParser : Parser
    {
        int columns;
        int rows;

        public DigitParser(string _path)
        {
            path = _path;
            ReadHeader();
        }
        override protected void ReadHeader()
        {
            using (FileStream stream = File.Open(path, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                magicNumber = SwapEndianness(reader.ReadInt32());
                samplesCount = SwapEndianness(reader.ReadInt32());
                columns = SwapEndianness(reader.ReadInt32());
                rows = SwapEndianness(reader.ReadInt32());
                sampleSize = columns * rows;
                offSet = reader.BaseStream.Position;
                headerOffSet = reader.BaseStream.Position;
            }
        }

        public void SaveDigitToBmp(byte[] digit)
        {
            string path = "C:\\Users\\Maciek\\Documents\\Visual Studio 2017\\Projects\\Magisterka\\Magisterka\\digit.bmp";
            SaveAsBitmap(path, digit);
        } 

        private void SaveAsBitmap(string fileName, byte[] imageData)
        {
            // Need to copy our 8 bit greyscale image into a 32bit layout.
            // Choosing 32bit rather than 24 bit as its easier to calculate stride etc.
            // This will be slow enough and isn't the most efficient method.
            var data = new byte[sampleSize * 4];

            int o = 0;

            for (var i = 0; i < sampleSize; i++)
            {
                var value = imageData[i];

                // Greyscale image so r, g, b, get the same
                // intensity value.
                data[o++] = value;
                data[o++] = value;
                data[o++] = value;
                data[o++] = 0;  // Alpha isn't actually used
            }

            unsafe
            {
                fixed (byte* ptr = data)
                {
                    // Craete a bitmap wit a raw pointer to the data
                    using (Bitmap image = new Bitmap(columns, rows, columns * 4,
                                PixelFormat.Format32bppRgb, new IntPtr(ptr)))
                    {
                        // And save it.
                        image.Save(Path.ChangeExtension(fileName, ".bmp"));
                    }
                }
            }
        }
        public int GetHeight()
        {
            return columns;
        }
        public int GetWidth()
        {
            return rows;
        }

        public double[] GetNextInput()
        {
            return ByteArrayToDoubleArray(ReadNext());
        }
    }
}
