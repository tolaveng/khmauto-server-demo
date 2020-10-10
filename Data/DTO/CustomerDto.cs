using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class CustomerDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Abn { get; set; }

        public ICollection<InvoiceDto> Invoices { get; set; }
        public ICollection<QuoteDto> Quotes { get; set; }

        public CustomerDto() { }
    }
}
