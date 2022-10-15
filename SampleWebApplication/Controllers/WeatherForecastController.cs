using Dapr.Client;
using Man.Dapr.Sidekick;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;

namespace SampleWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DaprClient _daprClient;
        private readonly IDaprSidecarHost _daprSidecarHost;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DaprClient daprClient, IDaprSidecarHost daprSidecarHost)
        {
            _logger = logger;
            _daprClient = daprClient;
            _daprSidecarHost = daprSidecarHost;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            return await _daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(
                HttpMethod.Get,
                "samplewebapi",
                "weatherforecast"
                );
        }

        [HttpGet("status")]
        public ActionResult GetStatus() => Ok(new
        {
            process = _daprSidecarHost.GetProcessInfo(),   // Information about the sidecar process such as if it is running
            options = _daprSidecarHost.GetProcessOptions() // The sidecar options if running, including ports and locations
        });
    }
}