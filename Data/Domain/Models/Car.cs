using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Models
{
    public class Car
    {
        public long CarId { get; set; }
        public string PlateNo { get; set; }
        public string CarModel { get; set; }
        public string CarMake { get; set; }
        public int CarYear { get; set; }
        public long ODO { get; set; }


        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Quote> Quotes { get; set; }

        public Car() { }
    }

}
