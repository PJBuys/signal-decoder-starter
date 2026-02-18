using SignalDecoder.Application.Interfaces;
using SignalDecoder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalDecoder.Application.Services
{
    public class SignalSimulatorService : ISignalSimulatorService
    {
        private readonly Random _random = new();
        public SimulateResponse Simulate(Dictionary<string, int[]> devices)
        {
            int totalDevices = devices.Count;
            int signalLength = devices.First().Value.Length;

            int activeCount = _random.Next(1, totalDevices + 1);

            List<KeyValuePair<string, int[]>> shuffled = devices.OrderBy(x => _random.Next()).ToList();
            List<KeyValuePair<string, int[]>> activeDevices = shuffled.Take(activeCount).ToList();

            int[] receivedSignalSum = new int[signalLength];
            for (int i = 0; i < signalLength; i++)
            {
                int Sum = 0;
                foreach (var Device in activeDevices)
                {
                    Sum = Sum + Device.Value[i];
                }
                receivedSignalSum[i] = Sum;

            }

            return new SimulateResponse
            {
                ReceivedSignal = receivedSignalSum,
                ActiveDeviceCount = activeCount,
                SignalLength = signalLength,
                TotalDevices = totalDevices
            };
        }
    }
}
