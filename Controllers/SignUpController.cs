using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskTrackerProject.Data;
using TaskTrackerProject.Models;

namespace TaskTrackerProject.Controllers
{
    [ApiController]
    [Route("api/signup")]
    public class SignUpController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public SignUpController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SignUp model)
        {
            try
            {
              _appDbContext.Add(model);
              await _appDbContext.SaveChangesAsync();  
            }
            catch (Exception ex)
            {
               return Ok(new { Status = false, Message = ex.Message }); 
            }
            return Ok(new { Status = true, Message = "Data saved successfully" });
        }
    }
}