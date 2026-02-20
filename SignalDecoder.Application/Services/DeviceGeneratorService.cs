using SignalDecoder.Application.Interfaces;
using SignalDecoder.Application.Validation;

namespace SignalDecoder.Application.Services
{
    public class DeviceGeneratorService : IDeviceGenertorService
    {
        public Dictionary<string, int[]> GenerateDevices(int count, int signalLength, int maxStrength)
        {
            RequestValidation.ValidateGenerateParams(count, signalLength, maxStrength);

            Dictionary<string, int[]> devices = new Dictionary<string, int[]>();

            for (int i = 1; i <= count; i++)
            {
                string id = $"d{i:D2}";
                int[] pattern = new int[signalLength];

                for (int j = 0; j < signalLength; j++)
                {
                    pattern[j] = Random.Shared.Next(0, maxStrength + 1);
                }

                devices[id] = pattern;
            }

            return devices;
        }
    }
}
