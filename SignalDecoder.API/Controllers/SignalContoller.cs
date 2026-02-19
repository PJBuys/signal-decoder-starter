using Microsoft.AspNetCore.Mvc;
using SignalDecoder.Application.Interfaces;
using SignalDecoder.Domain.Entities;

namespace SignalDecoder.API.Controllers
{
    [ApiController]
    [Route("api/signal")]
    public class SignalContoller : ControllerBase
    {
        private readonly ISignalSimulatorService _signalSimulatorService;
        private readonly ISignalDecoderService _signalDecoderService;
        public SignalContoller(ISignalSimulatorService signalSimulatorService, ISignalDecoderService SignalDecoderService)
        {
            _signalSimulatorService = signalSimulatorService;
            _signalDecoderService = SignalDecoderService;
        }

        [HttpPost("simulate")]
        public IActionResult SimulateSignal([FromBody] SimulateRequest simulateRequest)
        {
            SimulateResponse result = _signalSimulatorService.Simulate(simulateRequest.Devices);
            return Ok(result);
        }

        [HttpPost("decode")]
        public IActionResult DecodeSignal([FromBody] DecodeRequest decodeRequest)
        {
            DecodeResponse response = _signalDecoderService.Decode(decodeRequest);
            return Ok(response);
        }
    }
}
