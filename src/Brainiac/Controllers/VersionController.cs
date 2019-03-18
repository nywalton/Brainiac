using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Brainiac.Controllers
{
    /// <summary>
    /// Version Controller
    /// </summary>
    [Route("api/[controller]")]
    public class VersionController : Controller
    {
        private readonly AppConfig _config;

        /// <summary>
        /// VersionController constructor.
        /// </summary>
        /// <param name="options"></param>
        public VersionController(IOptions<AppConfig> options)
        {
            _config = options.Value;
        }

        /// <summary>
        /// Retrieve the version.
        /// </summary>
        /// <returns>The version of the application.</returns>
        [HttpGet]
        public string Version()
        {
            return _config.Version;
        }
    }
}
