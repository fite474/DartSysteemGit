﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartssystemServer.Network
{
    public class Datagram
    {
        public DataType DataType { get; set; }
        public dynamic Data { get; set; }
    }
}

