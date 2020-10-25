using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class CarDto
    {
        public long CarId { get; set; }
        public string PlateNo { get; set; }
        public string CarModel { get; set; }
        public string CarMake { get; set; }
        public int CarYear { get; set; }

        public long ODO { get; set; }

        public CarDto() { }
    }
}
