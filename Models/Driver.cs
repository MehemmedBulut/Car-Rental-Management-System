using RentalFinal.Helpers;
using RentalFinal.Migrations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static RentalFinal.Helpers.Helper;

namespace RentalFinal.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDeactive { get; set; }
        [Required(ErrorMessage ="Cinsiyyət tələb olunur")]
        public Gender Gender { get; set; }
        public List<Rent> Rents { get; set; }
    }
}
