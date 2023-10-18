using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RentalFinal.DAL;
using RentalFinal.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RentalFinal.Controllers
{
    [Authorize(Roles = "Admin,ContentManager")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Car> car = await _db.Cars.ToListAsync();
            return View(car);
        }



        public IActionResult Error()
        {
            return View();
        }
    }
}
