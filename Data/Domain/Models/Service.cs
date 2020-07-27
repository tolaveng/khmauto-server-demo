using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Models
{
    public class Service
    {
        public long Id { get; set; }
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        public int ServiceQty { get; set; }

        public long InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public long QuoteId { get; set; }
        public Quote Quote { get; set; }

        protected Service() { }
    }
}
