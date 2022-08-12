using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CrudDemoTwo.Models;

namespace CrudDemoTwo.Controllers;

public class HomeController : Controller
{
    public IActionResult Privacy()
    {
        return View();
    }

}
