using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Squad.AutofacTest.Models.Interfaces;

namespace Squad.AutofacTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ITodoModelBuilder _todoModelBuilder;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITodoModelBuilder todoModelBuilder)
        {
            _logger = logger;
            _todoModelBuilder = todoModelBuilder;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                Name = _todoModelBuilder.Erweiternt($"{Summaries.Length} Sum")
            })
            .ToArray();
        }
    }
}
