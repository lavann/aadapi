using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Dentsu.Aegis.Api.Controllers
{
    [ApiController]    
    [Authorize]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [Route("current")]
        [HttpGet]
        public IActionResult Get()
        {
            var user = User.Claims;

            var groups = User.Claims.Where(c => c.Type == "groups").Select( c=> c.Value).ToList();
            var userName = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
           // SecurityGroup = groups
            var rng = new Random();
            var forecasts =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                                              
            })
            .ToArray();

            return Ok(new
            {
                User = userName
                ,
                SecurityGroup = groups
                ,Forcasts = forecasts
            });
        }

        [Route("admin")]
        [Authorize("DensuAegisReportsAdmin")]
        public IActionResult GetForcastsForAdmin()
        {
            var user = User.Claims;

            var groups = User.Claims.Where(c => c.Type == "groups").Select(c => c.Value).ToList();
            var userName = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            // SecurityGroup = groups
            var rng = new Random();
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],

            })
            .ToArray();

            return Ok(new
            {
                User = userName
                ,
                SecurityGroup = groups
                ,
                Forcasts = forecasts
            });
        }
    }
}
