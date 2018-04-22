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
    class DigitParser
    {
        int magicNumber;
        int samplesCount;
        int rows;
        int columns;
        long offSet;
        long headerOffSet;
        string path;
        int currentDigitNumber = 0;

        public DigitParser(string _path)
        {
            path = _path;
        }
        public void ReadDigitsHeader()
        {
            using (FileStream stream = File.Open(path, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                magicNumber = SwapEndianness(reader.ReadInt32());
                samplesCount = SwapEndianness(reader.ReadInt32());
                columns = SwapEndianness(reader.ReadInt32());
                rows = SwapEndianness(reader.ReadInt32());
                offSet = reader.BaseStream.Position;
                headerOffSet = reader.BaseStream.Position;
            }
        }

        public byte[,] ReadNextDigit()
        {
            using (FileStream stream = File.Open(path, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                if (currentDigitNumber < samplesCount)
                {
                    return GetDigitFromFile(reader);
                }
                else
                {
                    currentDigitNumber = 0;
                    offSet = headerOffSet;
                    return GetDigitFromFile(reader);
                }
            }            
        }

        private byte[,] GetDigitFromFile(BinaryReader reader)
        {
            reader.BaseStream.Seek(offSet, SeekOrigin.Begin);
            byte[,] digit = new byte[columns, rows];

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    digit[j, i] = reader.ReadByte();
                }
            }
            offSet = reader.BaseStream.Position;
            currentDigitNumber++;
            return digit;
        }

        public void SaveDigitToBmp(byte[,] digit)
        {
            string path = "C:\\Users\\Maciek\\Documents\\Visual Studio 2017\\Projects\\Magisterka\\Magisterka\\digit.bmp";
            byte[] digitInArray = MatrixToArray(digit);

            SaveAsBitmap(path, digitInArray);
        } 

        private void SaveAsBitmap(string fileName, byte[] imageData)
        {
            // Need to copy our 8 bit greyscale image into a 32bit layout.
            // Choosing 32bit rather than 24 bit as its easier to calculate stride etc.
            // This will be slow enough and isn't the most efficient method.
            var data = new byte[columns * rows * 4];

            int o = 0;

            for (var i = 0; i < columns * rows; i++)
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

        public byte[] MatrixToArray(byte[,] digit)
        {
            byte[] array = new byte[columns*rows];
            int k = 0;
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    array[k] = digit[j, i];
                    k++;
                }
            }
            return array;
        }

        public double[] ByteArrayToDoubleArray(byte[] digit)
        {
            double[] doubleArray = new double[rows * columns];
            for(int i = 0; i < columns*rows; i++)
            {
                doubleArray[i] = digit[i];
            }
            return doubleArray;
        }

        public long GetOffSet()
        {
            return offSet;
        }

        private int SwapEndianness(int value)
        {
            var b1 = (value >> 0) & 0xff;
            var b2 = (value >> 8) & 0xff;
            var b3 = (value >> 16) & 0xff;
            var b4 = (value >> 24) & 0xff;

            return b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0;
        }

        public int GetMagicNumber()
        {
            return magicNumber;
        }

        public int GetSampleCount()
        {
            return samplesCount;
        }

        public int GetHeight()
        {
            return columns;
        }

        public int GetWidth()
        {
            return rows;
        }
    }
}
