using SignalDecoder.Domain.Exceptions;

namespace SignalDecoder.Application.Validation
{
    internal class RequestValidation
    {
        public static void ValidateGenerateParams(int count, int signalLength, int maxStrength)
        {
            var errors = new Dictionary<string, string[]>();

            if (count < 1 || count > 100)
                errors.Add("count", new[] { "Count must be between 1 and 100." });

            if (signalLength < 1 || signalLength > 20)
                errors.Add("signalLength", new[] { "Signal length must be between 1 and 20." });

            if (maxStrength < 1 || maxStrength > 100)
                errors.Add("maxStrength", new[] { "Max strength must be between 1 and 100." });

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }

        public static void ValidateSimulateRequest(Dictionary<string, int[]> devices)
        {
            var errors = new Dictionary<string, string[]>();

            if (devices is null || devices.Count == 0)
            {
                errors.Add("devices", new[] { "Devices must not be null or empty." });
                throw new ValidationException(errors);
            }

            int? expectedLength = null;
            foreach (var (id, pattern) in devices)
            {
                if (pattern is null || pattern.Length == 0)
                {
                    errors.Add($"devices.{id}", new[] { $"Device '{id}' has an empty or null signal pattern." });
                    continue;
                }

                if (expectedLength is null)
                    expectedLength = pattern.Length;
                else if (pattern.Length != expectedLength)
                    errors.Add($"devices.{id}.length",
                        new[] { $"Device '{id}' pattern length ({pattern.Length}) doesn't match expected length ({expectedLength})." });

                if (pattern.Any(v => v < 0))
                    errors.Add($"devices.{id}.values", new[] { $"Device '{id}' contains negative signal values." });
            }

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }

        public static void ValidateDecodeRequest(
            Dictionary<string, int[]> devices, int[] receivedSignal, int tolerance)
        {
            var errors = new Dictionary<string, string[]>();

            if (devices is null || devices.Count == 0)
                errors.Add("devices", new[] { "Devices must not be null or empty." });

            if (receivedSignal is null || receivedSignal.Length == 0)
                errors.Add("receivedSignal", new[] { "Received signal must not be null or empty." });

            if (tolerance < 0)
                errors.Add("tolerance", new[] { "Tolerance must be non-negative." });

            if (errors.Count > 0)
                throw new ValidationException(errors);

            int signalLength = receivedSignal!.Length;
            foreach (var (id, pattern) in devices!)
            {
                if (pattern is null || pattern.Length == 0)
                {
                    errors.Add($"devices.{id}", new[] { $"Device '{id}' has an empty or null signal pattern." });
                    continue;
                }

                if (pattern.Length != signalLength)
                    errors.Add($"devices.{id}.length",
                        new[] { $"Device '{id}' pattern length ({pattern.Length}) doesn't match received signal length ({signalLength})." });

                if (pattern.Any(v => v < 0))
                    errors.Add($"devices.{id}.values", new[] { $"Device '{id}' contains negative signal values." });
            }

            var lengths = devices.Values.Where(p => p != null).Select(p => p.Length).Distinct().ToList();
            if (lengths.Count > 1)
                errors.Add("devices.patternLength", new[] { "All device signal patterns must have the same length." });

            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
