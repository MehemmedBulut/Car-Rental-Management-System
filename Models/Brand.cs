using System.Collections.Generic;

namespace RentalFinal.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool İsDeactive { get; set; }
        public List<Ctype> Types { get; set; }
        public List<Car> Cars { get; set; }
    }
}
