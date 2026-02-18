using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalDecoder.Domain.Entities
{
    internal class DecodeResponse
    {
        public List<DecodeResult> Solutions { get; set; } = new();
        public int SolutionCount { get; set; }
        public long SolveTimeMs { get; set; }
    }
}
