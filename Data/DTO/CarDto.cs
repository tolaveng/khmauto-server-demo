using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class CarDto
    {
        public string CarNo { get; set; }
        public string CarModel { get; set; }
        public string CarMake { get; set; }
        public int CarYear { get; set; }

        public long ODO { get; set; }

        public string Color { get; set; }

        public CarDto() { }
    }
}
