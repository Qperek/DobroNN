using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magisterka
{
    class ByteArrayToByteException : Exception
    {
        public ByteArrayToByteException(string message)
        : base(message)
    {
        }
    }
}
