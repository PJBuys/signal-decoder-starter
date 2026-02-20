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

        /// <summary>
        /// Simulates a signal transmission by randomly selecting active devices.
        /// </summary>
        [HttpPost("simulate")]
        [ProducesResponseType(typeof(SimulateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public IActionResult SimulateSignal([FromBody] SimulateRequest simulateRequest)
        {
            SimulateResponse result = _signalSimulatorService.Simulate(simulateRequest.Devices);
            return Ok(result);
        }

        /// <summary>
        /// Decodes a received signal to identify which devices were transmitting.
        /// Returns all valid combinations of transmitting devices.
        /// </summary>
        /// 
        [HttpPost("decode")]
        [ProducesResponseType(typeof(DecodeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public IActionResult DecodeSignal([FromBody] DecodeRequest decodeRequest)
        {
            DecodeResponse response = _signalDecoderService.Decode(decodeRequest);
            return Ok(response);
        }
    }
}
