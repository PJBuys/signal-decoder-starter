using FluentAssertions;
using SignalDecoder.Application.Services;
using Xunit;

namespace SignalDecoder.Tests
{
    public class SignalSumulatorServiceTests
    {
        private readonly SignalSimulatorService _sut = new();

        [Fact]
        public void Simulate_ReturnsReceivedSignalWithCorrectLength()
        {
            var devices = new Dictionary<string, int[]>
        {
            { "Device_A", new[] { 1, 2, 3, 4 } },
            { "Device_B", new[] { 5, 6, 7, 8 } }
        };

            var result = _sut.Simulate(devices);

            result.ReceivedSignal.Should().HaveCount(4);
        }

        [Fact]
        public void Simulate_ReceivedSignalIsSumOfActiveDevices()
        {
            var devices = new Dictionary<string, int[]>
        {
            { "Device_A", new[] { 1, 0, 0, 0 } },
            { "Device_B", new[] { 0, 1, 0, 0 } }
        };

            var result = _sut.Simulate(devices);

            for (int i = 0; i < result.ReceivedSignal.Length; i++)
            {
                var maxPossible = devices.Values.Sum(v => v[i]);
                result.ReceivedSignal[i].Should().BeGreaterThanOrEqualTo(0);
                result.ReceivedSignal[i].Should().BeLessThanOrEqualTo(maxPossible);
            }
        }
    }
}
