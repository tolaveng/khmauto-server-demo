using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Models
{
    public class Customer
    {
        public long CustomerId { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Abn { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Quote> Quotes { get; set; }

        public Customer() { }
    }
}
