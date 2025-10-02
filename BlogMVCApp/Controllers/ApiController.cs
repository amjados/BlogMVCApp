using Microsoft.AspNetCore.Mvc;

namespace BlogMVCApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        /// <summary>
        /// Get application information
        /// </summary>
        /// <returns>Basic application info</returns>
        [HttpGet("info")]
        public IActionResult GetInfo()
        {
            return Ok(new
            {
                Application = "BlogMVCApp",
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                MachineName = Environment.MachineName,
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Get environment variables
        /// </summary>
        /// <returns>Environment information</returns>
        [HttpGet("environment")]
        public IActionResult GetEnvironment()
        {
            return Ok(new
            {
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                OSVersion = Environment.OSVersion.ToString(),
                ProcessId = Environment.ProcessId,
                WorkingDirectory = Environment.CurrentDirectory
            });
        }
    }
}