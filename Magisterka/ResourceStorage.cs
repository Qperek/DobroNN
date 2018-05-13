using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magisterka
{
    static class ResourceStorage
    {
        public static string trainDigitsPath = "C:\\Users\\Maciek\\documents\\visual studio 2017\\Projects\\Magisterka\\Magisterka\\Data\\train-images.idx3-ubyte";
        public static string trainLabelsPath = "C:\\Users\\Maciek\\documents\\visual studio 2017\\Projects\\Magisterka\\Magisterka\\Data\\train-labels.idx1-ubyte";
        public static string testDigitsPath = "C:\\Users\\Maciek\\documents\\visual studio 2017\\Projects\\Magisterka\\Magisterka\\Data\\t10k-images.idx3-ubyte";
        public static string testLabelsPath = "C:\\Users\\Maciek\\documents\\visual studio 2017\\Projects\\Magisterka\\Magisterka\\Data\\t10k-labels.idx1-ubyte";
        public static string errorFilePath = "C:\\Users\\Maciek\\documents\\visual studio 2017\\Projects\\Magisterka\\Magisterka\\Data\\errors.txt";
    }
}
