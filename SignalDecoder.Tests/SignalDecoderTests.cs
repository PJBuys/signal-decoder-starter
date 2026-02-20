using FluentAssertions;
using SignalDecoder.Application.Services;
using SignalDecoder.Domain.Entities;
using Xunit;

namespace SignalDecoder.Tests
{
    public class SignalDecoderTests
    {
        private readonly SignalDecoderService _sut = new();

        [Fact]
        public void Decoder_DecodesWithinTimeLimit_5Devices()
        {
            var devices = new Dictionary<string, int[]>
            {
                { "Device_A", new[] { 2, 4, 1, 3 } },
                { "Device_B", new[] { 7, 1, 5, 2 } },
                { "Device_C", new[] { 3, 6, 2, 8 } },
                { "Device_D", new[] { 1, 0, 9, 4 } },
                { "Device_E", new[] { 5, 2, 3, 1 } },
            };
            var receivedSignal = new[] { 10, 12, 6, 12 };

            var request = new DecodeRequest
            {
                Devices = devices,
                ReceivedSignal = receivedSignal
            };
            var result = _sut.Decode(request);

            result.Should().NotBeNull();
            result.Solutions[0].TransmittingDevices.Should().HaveCountGreaterThan(1);
        }

        [Fact]
        public void Decoder_DecodesWithinTimeLimit_10Devices()
        {
            var devices = new Dictionary<string, int[]>
            {
                { "d01", new[] { 9, 9, 2, 1 } },
                { "d02", new[] { 2, 6, 1, 3 } },
                { "d03", new[] { 7, 7, 9, 6 } },
                { "d04", new[] { 1, 1, 7, 1 } },
                { "d05", new[] { 9, 5, 2, 1 } },
                { "d06", new[] { 1, 6, 5, 3 } },
                { "d07", new[] { 0, 4, 5, 5 } },
                { "d08", new[] { 1, 5, 7, 7 } },
                { "d09", new[] { 7, 0, 9, 8 } },
                { "d10", new[] { 6, 0, 4, 5 } },
            };
            var receivedSignal = new[] { 43, 43, 51, 40 };

            var request = new DecodeRequest
            {
                Devices = devices,
                ReceivedSignal = receivedSignal
            };
            var result = _sut.Decode(request);

            result.Should().NotBeNull();
            result.SolveTimeMs.Should().BeLessThan(1000);
        }

        [Fact]
        public void Decoder_DecodesWithinTimeLimit_15Devices()
        {
            var devices = new Dictionary<string, int[]>
            {
                { "d01", new[] { 3, 7, 2, 5 } },
                { "d02", new[] { 8, 1, 6, 3 } },
                { "d03", new[] { 4, 9, 1, 7 } },
                { "d04", new[] { 6, 2, 8, 4 } },
                { "d05", new[] { 1, 5, 3, 9 } },
                { "d06", new[] { 7, 4, 9, 2 } },
                { "d07", new[] { 2, 8, 5, 6 } },
                { "d08", new[] { 9, 3, 7, 1 } },
                { "d09", new[] { 5, 6, 4, 8 } },
                { "d10", new[] { 3, 1, 2, 5 } },
                { "d11", new[] { 8, 7, 6, 4 } },
                { "d12", new[] { 4, 2, 9, 3 } },
                { "d13", new[] { 6, 5, 1, 7 } },
                { "d14", new[] { 1, 9, 8, 2 } },
                { "d15", new[] { 7, 4, 3, 6 } },
            };
            var receivedSignal = new[] { 74, 73, 74, 72 };

            var request = new DecodeRequest
            {
                Devices = devices,
                ReceivedSignal = receivedSignal
            };
            var result = _sut.Decode(request);

            result.Should().NotBeNull();
            result.SolveTimeMs.Should().BeLessThan(3000);
        }

        [Fact]
        public void Decoder_DecodesWithinTimeLimit_20Devices()
        {
            var devices = new Dictionary<string, int[]>
            {
                { "d01", new[] { 5, 3, 8, 2 } },
                { "d02", new[] { 1, 7, 4, 6 } },
                { "d03", new[] { 9, 2, 6, 3 } },
                { "d04", new[] { 3, 8, 1, 7 } },
                { "d05", new[] { 7, 4, 9, 5 } },
                { "d06", new[] { 2, 6, 3, 8 } },
                { "d07", new[] { 8, 1, 7, 4 } },
                { "d08", new[] { 4, 9, 2, 6 } },
                { "d09", new[] { 6, 5, 8, 1 } },
                { "d10", new[] { 3, 7, 4, 9 } },
                { "d11", new[] { 9, 2, 6, 3 } },
                { "d12", new[] { 1, 8, 3, 7 } },
                { "d13", new[] { 7, 4, 9, 2 } },
                { "d14", new[] { 5, 6, 1, 8 } },
                { "d15", new[] { 2, 3, 7, 4 } },
                { "d16", new[] { 8, 9, 5, 6 } },
                { "d17", new[] { 4, 1, 8, 3 } },
                { "d18", new[] { 6, 7, 2, 9 } },
                { "d19", new[] { 3, 5, 6, 1 } },
                { "d20", new[] { 9, 4, 7, 5 } },
            };
            var receivedSignal = new[] { 102, 101, 106, 99 };

            var request = new DecodeRequest
            {
                Devices = devices,
                ReceivedSignal = receivedSignal
            };
            var result = _sut.Decode(request);

            result.Should().NotBeNull();
            result.SolveTimeMs.Should().BeLessThan(5000);
        }

        [Fact]
        public void Decocer_EdgeCase_AllZeroSignal()
        {
            var devices = new Dictionary<string, int[]>
            {
                { "Device_A", new[] { 0, 0, 0, 0 } },
                { "Device_B", new[] { 0, 0, 0, 0 } }
            };
            var receivedSignal = new[] { 0, 0, 0, 0 };
            var request = new DecodeRequest
            {
                Devices = devices,
                ReceivedSignal = receivedSignal,
                Tolerance = 0
            };
            var result = _sut.Decode(request);
            result.Should().NotBeNull();
            result.SolutionCount.Should().Be(0);
            result.Solutions[0].DecodedSignals.Should().HaveCount(0);
            result.Solutions[0].TransmittingDevices.Should().HaveCount(0);
        }
    }
}
