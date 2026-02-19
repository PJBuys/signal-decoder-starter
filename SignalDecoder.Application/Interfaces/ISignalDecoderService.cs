using SignalDecoder.Domain.Entities;

namespace SignalDecoder.Application.Interfaces
{
    public interface ISignalDecoderService
    {
        DecodeResponse Decode(DecodeRequest decodeRequest);
    }
}
