using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalDecoder.Domain.Entities
{
    internal class SimulateResponse
    {
        public int[] ReceivedSignal { get; set; } = Array.Empty<int>();
        public int ActiveDeviceCount { get; set; }
        public int SignalLength { get; set; }
        public int TotalDevices { get; set; }
    }
}
