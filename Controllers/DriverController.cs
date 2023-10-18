using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalFinal.DAL;
using RentalFinal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalFinal.Controllers
{
    [Authorize(Roles = "Admin,ContentManager")]
    public class DriverController : Controller
    {
        private readonly AppDbContext _db;

        public DriverController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Driver> driver = await _db.Drivers.ToListAsync();
            return View(driver);
        }
        #region Create
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Driver driver)
        {
            #region Exist
            bool IsExist = await _db.Drivers.AnyAsync(x => x.PhoneNumber == driver.PhoneNumber);
            if (IsExist)
            {
                ModelState.AddModelError("Name", "Bu nömrə mövcuddur");
                return View();
            }
            #endregion
            await _db.AddAsync(driver);
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
            Driver dbDriver = await _db.Drivers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbDriver == null)
            {
                return BadRequest();
            }

            return View(dbDriver);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Driver driver)
        {
            if (id == null)
            {
                return NotFound();
            }
            Driver dbDriver = await _db.Drivers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbDriver == null)
            {
                return BadRequest();
            }
            #region Exist
            bool IsExist = await _db.Drivers.AnyAsync(x => x.Name == driver.Name && x.Id != id);
            if (IsExist)
            {
                ModelState.AddModelError("Name", "This name is already exist");
                return View();
            }
            #endregion

            dbDriver.Name = driver.Name;
            dbDriver.Surname = driver.Surname;
            dbDriver.Age = driver.Age;
            dbDriver.PhoneNumber = driver.PhoneNumber;
            
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

            Driver dbDriver = await _db.Drivers.FirstOrDefaultAsync(p => p.Id == id);

            if (dbDriver == null)
            {
                return NotFound();
            }

            return View(dbDriver);
        }


        // POST: Rent/Delete/Id
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Driver driver = _db.Drivers.Find(id);

            if (driver == null)
            {
                return NotFound();
            }


            _db.Drivers.Remove(driver);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Driver dbDriver = await _db.Drivers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbDriver == null)
            {
                return BadRequest();
            }

            return View(dbDriver);
        }
        #endregion

    }
}
