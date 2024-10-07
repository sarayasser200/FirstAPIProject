using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FirstAPIProject.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOptionsMonitor<AttachmentOptions> _attachmentOptions;

        public ConfigController(IConfiguration configuration, IOptionsMonitor<AttachmentOptions> attachmentOptions)
        {
            _configuration = configuration;
            _attachmentOptions = attachmentOptions;
            // value =attachmentOptions.Value;
        }
        [HttpGet]
        [Route("")]
        public IActionResult GetConfig() 
        {
            Thread.Sleep(10000);
            var config = new
            {
                EnvName = _configuration["ASPNETCORE_ENVIRONMENT"],
                AllowedHosts = _configuration["AllowedHosts"],
                DefaultConnection = _configuration["ConnectionStrings:DefaultConnection"],
                DefaultLogLevel = _configuration["Logging:LogLevel:Default"],
                TestKey = _configuration["TestKey"],
                SigningKey = _configuration["SigningKey"],
                AttachmentOptions = _attachmentOptions.CurrentValue

            };
            return Ok(config);
        }

    }
}
