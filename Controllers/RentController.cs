using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalFinal.DAL;
using RentalFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalFinal.Controllers
{
    [Authorize(Roles = "Admin,ContentManager")]
    public class RentController : Controller
    {
        private readonly AppDbContext _db;

        public RentController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Rent> rent = await _db.Rents.Include(x => x.Car).Include(x => x.Driver).Where(x=>x.Car.IsBusy).ToListAsync();
            return View(rent);
        }


        #region Create
        private async Task PopulateDropdowns()
        {
            ViewBag.Car = await _db.Cars.Where(x => !x.IsBusy).ToListAsync();
            ViewBag.Driver = await _db.Drivers.ToListAsync();
        }
        public async Task<IActionResult> Create()
        {

            await PopulateDropdowns();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rent rent, int nsId, int phoneId, int NumId)
        {

            ViewBag.Car = await _db.Cars.Where(x => !x.IsBusy).ToListAsync();
            ViewBag.Driver = await _db.Drivers.ToListAsync();

            Car dbCar = await _db.Cars.FirstOrDefaultAsync(x => x.Id == NumId);
            if (dbCar == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(rent);
            }

            TimeSpan rentTime = rent.ReturnDate - rent.RentDate;
            int gunSayi = (int)rentTime.TotalDays;

            rent.TotalPrice = gunSayi * dbCar.DailyPrice;
            dbCar.IsBusy = true;
            rent.DriverId = phoneId;
            rent.CarId = NumId;
            rent.DriverId = nsId;
            await _db.Rents.AddAsync(rent);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Rent rent = await _db.Rents.Include(x => x.Car).FirstOrDefaultAsync(x => x.Id == id);
            if (rent == null)
            {
                return BadRequest();
            }
            ViewBag.Car = await _db.Cars.ToListAsync();
            ViewBag.Driver = await _db.Drivers.ToListAsync();
            return View(rent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Rent rent, int nsId, int phoneId, int NumId)
        {

            ViewBag.Car = await _db.Cars.Where(x => !x.IsBusy).ToListAsync();
            ViewBag.Driver = await _db.Drivers.ToListAsync();
            Rent dbRent = await _db.Rents.Include(x => x.Car).FirstOrDefaultAsync(x => x.Id == id);
            if (dbRent == null)
            {
                return BadRequest();
            }
            Car dbCar = await _db.Cars.FirstOrDefaultAsync(x => x.Id == NumId);
            if (dbCar == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(rent);
            }

            TimeSpan rentTime = rent.ReturnDate - rent.RentDate;
            int gunSayi = (int)rentTime.TotalDays;

            rent.TotalPrice = gunSayi * dbCar.DailyPrice;
            dbRent.ReturnDate = rent.ReturnDate;
            dbRent.RentDate = rent.RentDate;
            dbRent.TotalPrice = rent.TotalPrice;
            dbRent.DriverId = nsId;
            dbRent.DriverId = phoneId;
            dbRent.CarId = NumId;


            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        // GET: Patient/Delete/Id
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rent dbRent = await _db.Rents.Include(x=>x.Car).FirstOrDefaultAsync(p => p.Id == id);
            
            if (dbRent == null)
            {
                return NotFound();
            }

            return View(dbRent);
        }


        // POST: Rent/Delete/Id
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Rent rent = _db.Rents.Find(id);

            if (rent == null)
            {
                return NotFound();
            }
            Car car = _db.Cars.FirstOrDefault(x=>x.Id==rent.CarId);
            car.IsBusy = false;

            //car.Id = rent.CarId;
            //car.IsBusy = false;

            
            _db.Rents.Remove(rent);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion


        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            Rent rent = await _db.Rents.
                Include(x=>x.Driver).
                Include(x=>x.Car).ThenInclude(x=>x.Brand).ThenInclude(x=>x.Types).FirstOrDefaultAsync(x => x.Id == id);
            return View(rent);
        }
        #endregion

    }
}
