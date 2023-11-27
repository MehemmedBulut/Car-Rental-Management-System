using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalFinal.DAL;
using RentalFinal.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace RentalFinal.Controllers
{
    [Authorize(Roles = "Admin,ContentManager,Moderator")]
    public class TypesController : Controller
    {
        private readonly AppDbContext _db;

        public TypesController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Brand = await _db.Brands.Where(x=>!x.İsDeactive).ToListAsync();
            List<Ctype> types = await _db.Types.Where(x=>!x.İsDeactive).ToListAsync();
            return View(types);
        }
        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Brand = await _db.Brands.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ctype type, int brandId)
        {
            ViewBag.Brand = await _db.Brands.ToListAsync();

            type.BrandId = brandId;
            await _db.Types.AddAsync(type);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Brand = await _db.Brands.ToListAsync();
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Ctype type,int brandId)
        {
            if (id == null)
            {
                return NotFound();
            }
            Ctype? dbType = await _db.Types.FirstOrDefaultAsync(x => x.Id == id);
            if (dbType == null)
            {
                return BadRequest();
            }
            #region isExist
            bool isExist = await _db.Types.AnyAsync(x => x.Name == type.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu model mövcuddur");
                return View();
            }
            #endregion
            
            dbType.Name = type.Name;
            dbType.BrandId = type.BrandId;
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

            Ctype dbType = await _db.Types.FirstOrDefaultAsync(p => p.Id == id);

            if (dbType == null)
            {
                return NotFound();
            }

            return View(dbType);
        }


        // POST: Rent/Delete/Id
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Ctype type = _db.Types.Find(id);

            if (type == null)
            {
                return NotFound();
            }


            _db.Types.Remove(type);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion
    }
}
