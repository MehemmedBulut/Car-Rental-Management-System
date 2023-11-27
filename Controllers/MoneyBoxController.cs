using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalFinal.DAL;
using RentalFinal.ViewModels;
using System;
using System.Data;
using System.Linq;

namespace RentalFinal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MoneyBoxController : Controller
    {
        private readonly AppDbContext _db;

        public MoneyBoxController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            DateTime cariTarix = DateTime.Now;
            int cariAy = cariTarix.Month;
            int carilIl = cariTarix.Year;

            
            float aylıkToplamKiraye = _db.Rents
                .Where(k => k.RentDate.Month == cariAy && k.RentDate.Year == carilIl)
                .Sum(k => k.TotalPrice);

            ViewBag.AylıkToplamKiraBedeli = aylıkToplamKiraye;

            DateTime bugun = DateTime.Now;
            DateTime otuzGunEvvel = bugun.AddDays(-30);

            float lastThirtyDaysTotalRent = _db.Rents
                .Where(k => k.RentDate >= otuzGunEvvel && k.RentDate <= bugun)
                .Sum(k => k.TotalPrice);

            ViewBag.OtuzGunEvvelkiGelir = lastThirtyDaysTotalRent;

            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);

            float dailyTotalRent = _db.Rents
                .Where(k => k.RentDate.Date == today)
                .Sum(k => k.TotalPrice);

            ViewBag.DailyTotalRent = dailyTotalRent;


            return View();
        }
    }
}
