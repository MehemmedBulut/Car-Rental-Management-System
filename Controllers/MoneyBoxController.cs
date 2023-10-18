using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalFinal.DAL;
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
            DateTime güncelTarih = DateTime.Now;
            int güncelAy = güncelTarih.Month;
            int güncelYıl = güncelTarih.Year;

            // Geçerli ay ve yıl için tüm Kiralama nesnelerinin TotalPrice toplamını hesaplayın
            float aylıkToplamKiraBedeli = _db.Rents
                .Where(k => k.RentDate.Month == güncelAy && k.RentDate.Year == güncelYıl)
                .Sum(k => k.TotalPrice);

            ViewBag.AylıkToplamKiraBedeli = aylıkToplamKiraBedeli; // Toplamı ViewBag içinde saklayın

            return View();
        }
    }
}
