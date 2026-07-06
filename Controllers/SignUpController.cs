using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskTrackerProject.Data;
using TaskTrackerProject.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskTrackerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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


        [HttpGet("get")]

        public async Task<IActionResult> get()
        {
            try
            {
                var signupData = await _appDbContext.SignUps.ToListAsync();
                return Ok(new { Status = true, Message = "Data fetched successfully", Data = signupData });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }
        [HttpGet("delete")]
        public async Task<IActionResult> delete(Guid id)

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
    }
}