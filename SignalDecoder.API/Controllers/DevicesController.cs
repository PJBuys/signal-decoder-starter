using Microsoft.AspNetCore.Mvc;
using SignalDecoder.Application.Interfaces;

namespace SignalDecoder.API.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DevicesController: ControllerBase
    {
        private readonly IDeviceGenertorService _deviceGenertorService;
        public DevicesController(IDeviceGenertorService deviceGenertorService)
        {
            _deviceGenertorService = deviceGenertorService;
        }

        /// <summary>
        /// Generates a set of random devices with signal patterns.
        /// </summary>
        /// <param name="count">Number of devices (0-100, default is 5)</param>
        /// <param name="signalLength">Length of signal pattern(1-20, default is 4)</param>
        /// <param name="maxStrength">Maximum signal value (1-100 default 9)</param>
        /// <returns></returns>
        [HttpGet("generate")]
        [ProducesResponseType(typeof(Dictionary<string, int[]>), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Generate([FromQuery] int count = 5, [FromQuery] int signalLength = 4, int maxStrength = 9) {
            var devices = _deviceGenertorService.GenerateDevices(count, signalLength, maxStrength);
            return Ok(devices);
        }
    }
}
