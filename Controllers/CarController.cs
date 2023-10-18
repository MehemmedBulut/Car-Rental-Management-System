
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using RentalFinal.DAL;
using RentalFinal.Helpers;
using RentalFinal.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RentalFinal.Controllers
{
    [Authorize(Roles = "Admin,ContentManager")]
    public class CarController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CarController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Car> cars = await _db.Cars.
                Include(x=>x.CarImages).
                Include(x=>x.Transmission).
                Include(x=>x.Fuel).
                Include(x=>x.Brand).
                ThenInclude(x=>x.Types).
                Where(x => !x.İsDeactive).ToListAsync();
            
            return View(cars);
        }
        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Brand = await _db.Brands.ToListAsync();
            ViewBag.Type = await _db.Types.ToListAsync();
            ViewBag.Fuel = await _db.Fuels.ToListAsync();
            ViewBag.Transmission = await _db.Transmissions.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car, int fId, int tId, int brandId, int typesId)
        {
            ViewBag.Brand = await _db.Brands.ToListAsync();
            ViewBag.Type = await _db.Types.ToListAsync();
            ViewBag.Fuel = await _db.Fuels.ToListAsync();
            ViewBag.Transmission = await _db.Transmissions.ToListAsync();
            #region SaveImagesFile
            if (car.Photos == null)
            {
                ModelState.AddModelError("Photo", "Please select file");
                return View();
            }
            List<CarImage> carImages = new List<CarImage>();
            foreach (IFormFile Photo in car.Photos)
            {
                CarImage carImage = new CarImage();
                if (!Photo.IsImage())
                {
                    ModelState.AddModelError("Photos", "Please select image file");
                    return View();
                }
                if (Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photos", "Max 1Mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "images", "cars");
                carImage.Url = await Photo.SaveFileAsync(folder);
                carImages.Add(carImage);
            }

            car.CarImages = carImages;
            #endregion
            #region Setler




            car.TypeId = typesId;
            car.BrandId = brandId;
            car.TransmissionId = tId;
            car.FuelId = fId;

            await _db.Cars.AddAsync(car);
            await _db.SaveChangesAsync();
            #endregion

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
            Car? dbCar = await _db.Cars.Include(x => x.CarImages).FirstOrDefaultAsync(x => x.Id == id);
            if (dbCar == null)
            {
                return BadRequest();
            }
            ViewBag.Brand = await _db.Brands.ToListAsync();
            ViewBag.Type = await _db.Types.ToListAsync();
            ViewBag.Fuel = await _db.Fuels.ToListAsync();
            ViewBag.Transmission = await _db.Transmissions.ToListAsync();
            return View(dbCar);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Car car, int fId, int tId, int brandId, int typesId)
        {
            if (id == null)
            {
                return NotFound();
            }
            Car? dbCar = await _db.Cars.Include(x => x.CarImages).FirstOrDefaultAsync(x => x.Id == id);
            if (dbCar == null)
            {
                return BadRequest();
            }
            ViewBag.Brand = await _db.Brands.ToListAsync();
            ViewBag.Type = await _db.Types.ToListAsync();
            ViewBag.Fuel = await _db.Fuels.ToListAsync();
            ViewBag.Transmission = await _db.Transmissions.ToListAsync();
            #region SaveImagesFile
            if (car.Photos != null)
            {
                List<CarImage> carImages = new List<CarImage>();
                foreach (IFormFile Photo in car.Photos)
                {
                    CarImage carImage = new CarImage();
                    if (!Photo.IsImage())
                    {
                        ModelState.AddModelError("Photos", "Please select image file");
                        return View();
                    }
                    if (Photo.IsOlder1Mb())
                    {
                        ModelState.AddModelError("Photos", "Max 1Mb");
                        return View();
                    }
                    string folder = Path.Combine(_env.WebRootPath, "assets", "images", "cars");
                    carImage.Url = await Photo.SaveFileAsync(folder);
                    carImages.Add(carImage);
                }

                dbCar.CarImages.AddRange(carImages);
            }

            #endregion
            #region Setler




            dbCar.TypeId = typesId;
            dbCar.BrandId = brandId;
            dbCar.TransmissionId = tId;
            dbCar.FuelId = fId;

            await _db.SaveChangesAsync();
            #endregion
            return RedirectToAction("Index");
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            Car car = await _db.Cars.
                Include(x=>x.Brand).
                Include(x=>x.Type).
                Include(x=>x.CarImages).
                Include(x=>x.Transmission).
                Include(x=>x.Fuel).FirstOrDefaultAsync(x=>x.Id==id);
            return View(car);
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

            Car dbCar = await _db.Cars.FirstOrDefaultAsync(p => p.Id == id);

            if (dbCar == null)
            {
                return NotFound();
            }

            return View(dbCar);
        }


        // POST: Rent/Delete/Id
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Car car = _db.Cars.Find(id);

            if (car == null)
            {
                return NotFound();
            }


            _db.Cars.Remove(car);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region GetCarModel
        public async Task<IActionResult> GetType(int mainId)
        {
            var ctypes = await _db.Types.Where(x => x.BrandId == mainId && !x.İsDeactive).ToListAsync();
            return PartialView("_GetTypePartial", ctypes);
        }
        #endregion

        #region DeleteImage
        public int DeleteImage(int id, int carId)
        {
            int count = _db.CarImages.Count(x => x.CarId == carId);
            if (count == 2)
            {
                return 1;
            }
            CarImage? carImage = _db.CarImages.FirstOrDefault(x => x.Id == id);
            _db.CarImages.Remove(carImage);
            _db.SaveChanges();

            return 0;
        }
        #endregion

    }
}
