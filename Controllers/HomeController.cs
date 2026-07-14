using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerProject.Models;
using TaskTrackerProject.TaskDbContext;

namespace TaskTrackerProject.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _appDbContext;

    public HomeController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IActionResult Index()
    {
        var model = new SignUp();
        return View(model);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> DoLogin([FromBody] SignUp model)
    {
        try
        {
            var user = await _appDbContext.SignUps
                .FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);

            if (user != null)
            {
                // Store the logged-in user's email in session so we know who they are on later requests
                HttpContext.Session.SetString("UserEmail", user.Email);

                return Ok(new { Status = true, Message = "Login Successful" });
            }
            else
            {
                return Ok(new { Status = false, Message = "Invalid email or password." });
            }
        }
        catch (Exception ex)
        {
            return Ok(new { Status = false, Message = ex.Message });
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
