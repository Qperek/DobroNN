﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magisterka
{
    class InRangeExeption : Exception
    {
        public InRangeExeption(string message)
        : base(message)
    {
        }
    }
}
