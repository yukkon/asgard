using Microsoft.AspNetCore.Mvc;
using Models;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace BackEndApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaveController : ControllerBase
    {
        private readonly Settings.Settings _settings;
        private IWebHostEnvironment _environment;

        public SaveController(IOptions<Settings.Settings> settings, IWebHostEnvironment env)
        {
            _settings = settings.Value;
            _environment = env;
        }

        [HttpPost]
        public IActionResult UpdateAsgardStats(Stats stats)
        {
            try
            {
                DateTime dt = DateTime.Now;
                Calendar cal = new CultureInfo("en-US").Calendar;
                int week = cal.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                var p = Path.Combine(_environment.WebRootPath, $"{week}.json");
                StreamWriter sw = System.IO.File.CreateText(p);
                sw.Write(stats.Data);
                sw.Close();

                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
