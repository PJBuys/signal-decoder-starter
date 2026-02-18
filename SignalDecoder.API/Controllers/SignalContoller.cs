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
        public SignalContoller(ISignalSimulatorService signalSimulatorService)
        {
            _signalSimulatorService = signalSimulatorService;
        }

        [HttpPost("simulate")]
        public IActionResult SimulateSignal([FromBody] SimulateRequest simulateRequest)
        {
            SimulateResponse result = _signalSimulatorService.Simulate(simulateRequest.Devices);
            return Ok(result);
        }

    }
}
