using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Models
{
    public class Car
    {
        public long Id { get; set; }
        public string CarNo { get; set; }
        public string CarModel { get; set; }
        public string CarType { get; set; }
        public int CarYear { get; set; }

        public string ODO { get; set; }


        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Quote> Quotes { get; set; }

        protected Car() { }
    }

}
