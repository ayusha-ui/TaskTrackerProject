using System;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerProject.TaskDbContext;

namespace TaskTrackerProject.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public AccountController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> profile()
        {
            try
            {
                // Get the logged-in user's email from session (set during login)
                var email = HttpContext.Session.GetString("UserEmail");

                if (string.IsNullOrEmpty(email))
                {
                    return Ok(new { Status = false, Message = "Not logged in" });
                }

                var user = await _appDbContext.SignUps
                    .FirstOrDefaultAsync(x => x.Email == email);

                if (user == null)
                {
                    return Ok(new { Status = false, Message = "User not found" });
                }

                return Ok(new
                {
                    Status = true,
                    Message = "",
                    Data = new
                    {
                        name = user.Name,
                        email = user.Email,
                        phone = user.Phone,
                        dob = user.DOB,
                        address = user.Address
                    }
                });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public IActionResult logout()
        {
            try
            {
                HttpContext.Session.Clear();
                return Ok(new { Status = true, Message = "Logged out" });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }
    }
}