using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CrudDemoTwo.Models;
using Microsoft.AspNetCore.Identity;

namespace CrudDemoTwo.Controllers;

public class UserController : Controller
{

    private int? uid
    {
        get
        {
            return HttpContext.Session.GetInt32("UUID");
        }
    }

    private bool loggedIn
    {
        get
        {
            return uid != null;
        }
    }


    private MyContext db;

    // here we can "inject" our context service into the constructor
    public UserController(MyContext context)
    {
        db = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {

        if (loggedIn)
        {
            return RedirectToAction("All", "Post");
        }

        return View("Index");
    }

    [HttpPost("/register")]
    public IActionResult Register(User newUser)
    {


        if (db.Users.Any(user => user.Email == newUser.Email))
        {
            ModelState.AddModelError("Email", "is taken");
        }

        if (ModelState.IsValid == false)
        {
            return Index();
        }

        PasswordHasher<User> hashBrowns = new PasswordHasher<User>();
        newUser.Password = hashBrowns.HashPassword(newUser, newUser.Password);

        db.Users.Add(newUser);
        db.SaveChanges();

        HttpContext.Session.SetInt32("UUID", newUser.UserId);
        HttpContext.Session.SetString("Name", newUser.FullName());
        return RedirectToAction("All", "Post");
    }

    [HttpPost("/login")]
    public IActionResult Login(LoginUser loginUser)
    {
        if (ModelState.IsValid == false)
        {
            return Index();
        }
        User? dbUser = db.Users.FirstOrDefault(user => user.Email == loginUser.LoginEmail);

        if (dbUser == null )
        {
            ModelState.AddModelError("LoginEnaim", "not found");
            return Index();
        }

        PasswordHasher<LoginUser> hashBrowns = new PasswordHasher<LoginUser>();
        PasswordVerificationResult pwCompareResult = hashBrowns.VerifyHashedPassword(loginUser, dbUser.Password, loginUser.LoginPassword);

        if (pwCompareResult == 0)
        {
            ModelState.AddModelError("LoginPassword", "is not correct");
            return Index();
        }

        HttpContext.Session.SetInt32("UUID", dbUser.UserId);
        HttpContext.Session.SetString("Name", dbUser.FullName());
        return RedirectToAction("All", "Post");
    }

    [HttpPost("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

}
