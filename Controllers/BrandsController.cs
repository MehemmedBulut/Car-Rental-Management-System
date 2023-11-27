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
    [Authorize(Roles = "Admin,ContentManager,Moderator")]
    public class BrandsController : Controller
    {
        private readonly AppDbContext _db;

        public BrandsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Brand> brands = await _db.Brands.Include(x=>x.Types).Where(x=>!x.İsDeactive).ToListAsync();
            return View(brands);
        }
        #region Create
        public async Task<IActionResult> Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            #region Exist
            bool IsExist = await _db.Brands.AnyAsync(x => x.Name == brand.Name);
            if (IsExist)
            {
                ModelState.AddModelError("Name", "Bu marka mövcuddur");
                return View();
            }
            #endregion
            await _db.AddAsync(brand);
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
            Brand dbBrand = await _db.Brands.FirstOrDefaultAsync(x => x.Id == id);
            if (dbBrand == null)
            {
                return BadRequest();
            }

            return View(dbBrand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Brand brand)
        {
            if (id == null)
            {
                return NotFound();
            }
            Brand dbBrand = await _db.Brands.FirstOrDefaultAsync(x => x.Id == id);
            if (dbBrand == null)
            {
                return BadRequest();
            }
            #region Exist
            bool IsExist = await _db.Brands.AnyAsync(x => x.Name == brand.Name && x.Id != id);
            if (IsExist)
            {
                ModelState.AddModelError("Name", "Bu Marka Mövcüddur");
                return View();
            }
            #endregion

            dbBrand.Name = brand.Name;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion


        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            //ViewBag.Type = await _db.Types.Where(x=>x.Id == id).ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Brand dbBrand = await _db.Brands.Include(x=>x.Types).FirstOrDefaultAsync(x => x.Id == id);
            if (dbBrand == null)
            {
                return BadRequest();
            }

            return View(dbBrand);
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

            Brand dbBrand = await _db.Brands.FirstOrDefaultAsync(p => p.Id == id);

            if (dbBrand == null)
            {
                return NotFound();
            }

            return View(dbBrand);
        }


        // POST: Rent/Delete/Id
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Brand brand = _db.Brands.Find(id);

            if (brand == null)
            {
                return NotFound();
            }


            _db.Brands.Remove(brand);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

    }
}
