using Microsoft.AspNetCore.Mvc;
using TaskTrackerProject.Service;

namespace TaskTrackerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("send")]
        public async Task<IActionResult> Send(string email)
        {
            var result = await _emailService.EmailSend(email);

            if (result)
                return Ok("Email sent successfully.");

            return BadRequest("Failed to send email.");
        }
    }
}