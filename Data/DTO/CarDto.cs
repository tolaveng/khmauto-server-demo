using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class CarDto
    {
        public long Id { get; set; }
        public string CarNo { get; set; }
        public string CarModel { get; set; }
        public string CarType { get; set; }
        public int CarYear { get; set; }

        public string ODO { get; set; }

        public CarDto() { }
    }
}
