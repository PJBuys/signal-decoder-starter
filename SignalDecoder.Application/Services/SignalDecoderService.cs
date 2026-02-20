using SignalDecoder.Application.Interfaces;
using SignalDecoder.Application.Validation;
using SignalDecoder.Domain.Entities;
using System.Diagnostics;

namespace SignalDecoder.Application.Services
{
    public class SignalDecoderService : ISignalDecoderService
    {
        public DecodeResponse Decode(DecodeRequest decodeRequest)
        {
            RequestValidation.ValidateDecodeRequest(
            decodeRequest.Devices, decodeRequest.ReceivedSignal, decodeRequest.Tolerance);

            Stopwatch stopwatch = Stopwatch.StartNew();

            int[] recieved = decodeRequest.ReceivedSignal;
            int tolerance = decodeRequest.Tolerance;
            int signalLength = recieved.Length;

            if (recieved.All(x => x == 0) && tolerance == 0)
            {
                stopwatch.Stop();
                return new DecodeResponse
                {
                    Solutions = new List<DecodeResult> {
                        new DecodeResult
                        {
                            TransmittingDevices = new List<string>(),
                            DecodedSignals = new Dictionary<string, int[]>(),
                            ComputedSum = new int[signalLength],
                            MatchesReceived = true
                        },
                    },
                    SolutionCount = 0,
                    SolveTimeMs = stopwatch.ElapsedMilliseconds
                };
            }

            var sortedDevices = decodeRequest.Devices
                .Select(d => (Id: d.Key, Pattern: d.Value, Total: d.Value.Sum()))
                .OrderByDescending(x => x.Total)
                .ToList();

            int deviceCount = sortedDevices.Count;

            int[][] suffixSum = new int[signalLength][];
            for (int pos = 0; pos < signalLength; pos++)
            {
                suffixSum[pos] = new int[deviceCount + 1];
                suffixSum[pos][deviceCount] = 0;
                for (int i = deviceCount - 1; i >= 0; i--)
                {
                    suffixSum[pos][i] = suffixSum[pos][i + 1] + sortedDevices[i].Pattern[pos];
                }
            }

            var solutions = new List<List<int>>();
            var currentSubset = new List<int>();
            int[] currentSum = new int[signalLength];

            Backtrack(
            index: 0,
            devices: sortedDevices,
            received: recieved,
            tolerance: tolerance,
            signalLength: signalLength,
            currentSum: currentSum,
            currentSubset: currentSubset,
            solutions: solutions,
            suffixSum: suffixSum,
            deviceCount: deviceCount);

            stopwatch.Stop();
            var response = new DecodeResponse
            {
                Solutions = new List<DecodeResult>(),
                SolutionCount = solutions.Count,
                SolveTimeMs = stopwatch.ElapsedMilliseconds
            };

            foreach (var solution in solutions)
            {
                var computedSum = new int[signalLength];
                var decodedSignals = new Dictionary<string, int[]>();
                var transmittingIds = new List<string>();

                foreach (int idx in solution)
                {
                    var device = sortedDevices[idx];
                    transmittingIds.Add(device.Id);
                    decodedSignals[device.Id] = device.Pattern;

                    for (int pos = 0; pos < signalLength; pos++)
                        computedSum[pos] += device.Pattern[pos];
                }

                response.Solutions.Add(new DecodeResult
                {
                    TransmittingDevices = transmittingIds.OrderBy(id => id).ToList(),
                    DecodedSignals = decodedSignals,
                    ComputedSum = computedSum,
                    MatchesReceived = IsWithinTolerance(computedSum, recieved, tolerance, signalLength)
                });
            }

            return response;
        }

        private void Backtrack(
        int index,
        List<(string Id, int[] Pattern, int Total)> devices,
        int[] received,
        int tolerance,
        int signalLength,
        int[] currentSum,
        List<int> currentSubset,
        List<List<int>> solutions,
        int[][] suffixSum,
        int deviceCount)
        {
            
            if (currentSubset.Count > 0 && IsWithinTolerance(currentSum, received, tolerance, signalLength))
            {
                solutions.Add(new List<int>(currentSubset));
            }

            for (int i = index; i < deviceCount; i++)
            {
                var pattern = devices[i].Pattern;

                bool overflowed = false;
                for (int pos = 0; pos < signalLength; pos++)
                {
                    currentSum[pos] += pattern[pos];

                    if (currentSum[pos] > received[pos] + tolerance)
                    {
                        for (int undo = pos; undo >= 0; undo--)
                            currentSum[undo] -= pattern[undo];
                        overflowed = true;
                        break;
                    }
                }

                if (overflowed)
                    continue;

                bool reachable = true;
                for (int pos = 0; pos < signalLength; pos++)
                {
                    int bestPossible = currentSum[pos] + suffixSum[pos][i + 1];
                    if (bestPossible < received[pos] - tolerance)
                    {
                        reachable = false;
                        break;
                    }
                }

                if (reachable)
                {
                    currentSubset.Add(i);
                    Backtrack(i + 1, devices, received, tolerance, signalLength,
                              currentSum, currentSubset, solutions, suffixSum, deviceCount);
                    currentSubset.RemoveAt(currentSubset.Count - 1);
                }

                for (int pos = 0; pos < signalLength; pos++)
                    currentSum[pos] -= pattern[pos];
            }
        }

        private static bool IsWithinTolerance(int[] computed, int[] received, int tolerance, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (Math.Abs(computed[i] - received[i]) > tolerance)
                    return false;
            }
            return true;
        }
    }
}
