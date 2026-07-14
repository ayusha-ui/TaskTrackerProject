using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerProject.TaskDbContext;

namespace TaskTrackerProject.Controllers
{
    [ApiController]
    [Route("api/task")]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public TaskController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // POST /api/task/save  (used by dashboard JS)
        // POST /api/task/add   (alias)
        [HttpPost("save")]
        [HttpPost("add")]
        public async Task<IActionResult> save([FromBody] TaskTrackerProject.Models.Task model)
        {
            try
            {
                model.Id = Guid.NewGuid();
                await _appDbContext.AddAsync(model);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { Status = true, Message = "Task Added Successfully" });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> get()
        {
            try
            {
                var taskData = await _appDbContext.Tasks.ToListAsync();
                return Ok(new { Status = true, Message = "", Data = taskData });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> update([FromBody] TaskTrackerProject.Models.Task model)
        {
            try
            {
                var taskExist = await _appDbContext.Tasks
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (taskExist != null)
                {
                    taskExist.Title = model.Title;
                    taskExist.Description = model.Description;
                    taskExist.Priority = model.Priority;
                    taskExist.Status = model.Status;
                    taskExist.DueDate = model.DueDate;
                    _appDbContext.Update(taskExist);
                    await _appDbContext.SaveChangesAsync();
                    return Ok(new { Status = true, Message = "Task Updated Successfully" });
                }
                else
                {
                    return Ok(new { Status = false, Message = "Task not found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }

        // Called by the inline status dropdown in the table
        [HttpPost("updateStatus")]
        public async Task<IActionResult> updateStatus([FromBody] TaskTrackerProject.Models.Task model)
        {
            try
            {
                var taskExist = await _appDbContext.Tasks
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (taskExist != null)
                {
                    taskExist.Status = model.Status;
                    _appDbContext.Update(taskExist);
                    await _appDbContext.SaveChangesAsync();
                    return Ok(new { Status = true, Message = "Status Updated" });
                }
                else
                {
                    return Ok(new { Status = false, Message = "Task not found" });
                }
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
                var dataExist = await _appDbContext.Tasks
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (dataExist != null)
                {
                    _appDbContext.Remove(dataExist);
                    await _appDbContext.SaveChangesAsync();
                    return Ok(new { Status = true, Message = "Task Deleted Successfully" });
                }
                else
                {
                    return Ok(new { Status = false, Message = "Task not found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }
    }
}
