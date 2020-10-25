using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class ServiceDto
    {
        public long ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        public int ServiceQty { get; set; }

        public long? InvoiceId { get; set; }
        public InvoiceDto Invoice { get; set; }

        public long? QuoteId { get; set; }
        public QuoteDto Quote { get; set; }

        public ServiceDto() { }
    }
}
