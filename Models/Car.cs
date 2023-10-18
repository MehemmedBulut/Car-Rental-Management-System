using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalFinal.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Bu xana boş ola bilməz")]
        public string Color { get; set; }
        public float DailyPrice { get; set; }
        public int PassYear { get; set; }
        public string CarNumber { get; set; }
        public Brand Brand { get; set; }
        public int BrandId { get; set; }
        [ForeignKey("TypeId")]
        public Ctype Type { get; set; }
        public int TypeId { get; set; }
        public Fuel Fuel { get; set; }
        public int FuelId { get; set; }
        public Transmission Transmission { get; set; }
        public int TransmissionId { get; set; }
        public bool İsDeactive { get; set; }
        public List<CarImage> CarImages { get; set; }
        [NotMapped]
        public IFormFile[] Photos { get; set; }
        public bool IsDeactive { get; set; }
        public bool IsBusy { get; set; }
        public List<Rent> Rents { get; set; }
    }
}
