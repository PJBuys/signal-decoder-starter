using FluentAssertions;
using SignalDecoder.Application.Services;
using Xunit;

namespace SignalDecoder.Tests
{
    public class DeviceGeneratorServiceTests
    {
        private readonly DeviceGeneratorService _sut = new();

        [Fact]
        public void GenerateDevices_ReturnsCorrectCount()
        {
            var devices = _sut.GenerateDevices(5, 4, 9);

            devices.Should().HaveCount(5);
        }

        [Fact]
        public void GenerateDevices_ReturnsCorrectSignalLength()
        {
            var devices = _sut.GenerateDevices(3, 6, 9);

            foreach (var kvp in devices)
            {
                kvp.Value.Should().HaveCount(6);
            }
        }

        [Fact]
        public void GenerateDevices_SignalValuesWithinRange()
        {
            var devices = _sut.GenerateDevices(5, 4, 9);

            foreach (var kvp in devices)
            {
                foreach (var val in kvp.Value)
                {
                    val.Should().BeInRange(0, 9);
                }
            }
        }

        [Fact]
        public void GenerateDevices_DeviceNamesAreUnique()
        {
            var devices = _sut.GenerateDevices(10, 4, 9);

            devices.Keys.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void GenerateDevices_WithOneDevice_ReturnsSingleDevice()
        {
            var devices = _sut.GenerateDevices(1, 4, 9);

            devices.Should().HaveCount(1);
        }

        [Fact]
        public void GenerateDevices_MaxStrengthOne_AllValuesZeroOrOne()
        {
            var devices = _sut.GenerateDevices(5, 4, 1);

            foreach (var kvp in devices)
            {
                foreach (var val in kvp.Value)
                {
                    val.Should().BeInRange(0, 1);
                }
            }
        }
    }
}
