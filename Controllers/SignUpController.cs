using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerProject.Models;
using TaskTrackerProject.Service;
using TaskTrackerProject.TaskDbContext;

namespace TaskTrackerProject.Controllers
{
    [ApiController]
    [Route("api/signup")]
    public class SignUpController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IEmailService _emailService;

        public SignUpController(AppDbContext appDbContext, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SignUp model)
        {
            try
            {
                _appDbContext.Add(model);
                await _emailService.EmailSend(model.Email); // Send email after saving the data   
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }

            return Ok(new { Status = true, Message = "Data Added Successfully" });

        }


        [HttpGet("get")]
        public async Task<IActionResult> get()
        {
            try
            {
                var signupData = await _appDbContext.SignUps.
                ToListAsync();
                return Ok(new
                {
                    Status = true,
                    Message = "",
                    Data = signupData
                });

            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }

        [HttpGet("delete")]
        public async Task<IActionResult> delete([FromQuery] Guid id)
        {
            try
            {
                var deleteData = await _appDbContext.SignUps.
                    FirstOrDefaultAsync(x => x.Id == id);
                if (deleteData != null)
                {
                    _appDbContext.Remove(deleteData);
                    await _appDbContext.SaveChangesAsync();
                    return Ok(new
                    {
                        Status = true,
                        Message = "Data Deleted Successfully"
                    });

                }
                else
                {
                    return Ok(new
                    {
                        Status = false,
                        Message = "Data Not Exist"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> update([FromBody] SignUp model)
        {
            try
            {
                var dataExist = await _appDbContext.SignUps.
                    FirstOrDefaultAsync(x => x.Id == model.Id);

                if (dataExist != null)
                {
                    dataExist.Name = model.Name;
                    dataExist.Address = model.Address;
                    dataExist.Email = model.Email;
                    dataExist.Phone = model.Phone;
                    dataExist.DOB = model.DOB;

                    _appDbContext.Update(dataExist);
                    await _appDbContext.SaveChangesAsync();
                    return Ok(new { Status = true, Message = "Data Updated Successfully" });
                }
                else
                {
                    return Ok(new { Status = false, Message = "Data Not Exist" });
                }

            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] SignUp model)
        {
            try
            {
                var user = await _appDbContext.SignUps
                    .FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);

                if (user != null)
                {
                    // Store the logged-in user's email in session so we know who they are on later requests
                    HttpContext.Session.SetString("UserEmail", user.Email);

                    return Ok(new
                    {
                        Status = true,
                        Message = "Login Successfull"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Status = false,
                        Message = "User Not Exist"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }
    }
}