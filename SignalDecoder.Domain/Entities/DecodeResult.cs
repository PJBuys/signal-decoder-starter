using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalDecoder.Domain.Entities
{
    internal class DecodeResult
    {
        public List<string> TransmittingDevices { get; set; } = new();
        public Dictionary<string, int[]> DecodedSignals { get; set; } = new();
        public int[] ComputedSum { get; set; } = Array.Empty<int>();
        public bool MatchesReceived { get; set; }
    }
}
