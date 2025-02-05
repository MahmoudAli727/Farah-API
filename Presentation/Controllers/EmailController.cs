using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;

        public EmailController(IEmailService emailService)
        {
            this.emailService = emailService;
        }


        //public async Task<IActionResult> SendEmail([FromForm] EmailDTO emailDTO)
        //{



        //    return Ok();
        //}
    }
}
