using System.Collections.Generic;

namespace RentalFinal.Models
{
    public class Ctype
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Brand Brand { get; set; }
        public int BrandId { get; set; }
        public bool İsDeactive { get; set; }
        public List<Car> Cars { get; set; }
    }
}
