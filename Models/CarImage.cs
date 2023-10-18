namespace RentalFinal.Models
{
    public class CarImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public Car Car { get; set; }
        public int CarId { get; set; }
    }
}
