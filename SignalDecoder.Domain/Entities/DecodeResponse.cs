namespace SignalDecoder.Domain.Entities
{
    public class DecodeResponse
    {
        public List<DecodeResult> Solutions { get; set; } = new();
        public int SolutionCount { get; set; }
        public long SolveTimeMs { get; set; }
    }
}
