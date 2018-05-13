using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magisterka
{
    class TextBoxParseException : Exception
    {
        public TextBoxParseException(string message)
        : base(message)
    {
        }
    }
}

