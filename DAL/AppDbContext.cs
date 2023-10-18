using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentalFinal.Models;

namespace RentalFinal.DAL
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Ctype> Types { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Transmission> Transmissions { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Rent> Rents { get; set; }
    }
}
