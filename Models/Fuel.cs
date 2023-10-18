using System.Collections.Generic;

namespace RentalFinal.Models
{
    public class Fuel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Car> Cars { get; set; }
    }
}
