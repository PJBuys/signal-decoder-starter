using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalDecoder.Domain.Entities
{
    internal class DecodeRequest
    {
        public Dictionary<string, int[]> Devices { get; set; } = new();
        public int[] ReceivedSignal { get; set; } = Array.Empty<int>();
        public int Tolerance { get; set; } = 0;
    }
}
