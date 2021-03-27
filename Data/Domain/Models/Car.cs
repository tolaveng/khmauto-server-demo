using Data.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Domain.Models
{
    public class Car
    {
        [key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CarNo { get; set; }
        public string CarModel { get; set; }
        public string CarMake { get; set; }
        public int CarYear { get; set; }
        public long ODO { get; set; }


        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Quote> Quotes { get; set; }

        public Car() { }
    }

}
