using RentalFinal.Helpers;
using System;

namespace RentalFinal.Models
{
    public class Rent
    {
        public int Id { get; set; }
        public Car Car { get; set; }
        public int CarId { get; set; }
        public Driver Driver { get; set; }
        public int DriverId { get; set; }
        public DateTime RentDate { get; set; }
        [TimeSlotTimeValidation(ErrorMessage = "Təhvil tarixi başlanğıc tarixindən kiçik ola bilməz!")]
        public DateTime ReturnDate { get; set;}
        public bool IsComplete { get; set; }
        public float TotalPrice { get; set; }
    }
}
