namespace SignalDecoder.Application.Interfaces
{
    public interface IDeviceGenertorService
    {
        Dictionary<string, int[]> GenerateDevices(int count, int signalLength, int maxStrength);
    }
}
