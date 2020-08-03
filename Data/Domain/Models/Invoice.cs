using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Models
{
    public class Invoice
    {
        public long Id { get; set; }
        public DateTime InvoiceDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaidDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public float Gst { get; set; }
        public string Note { get; set; }
        public string ODO { get; set; }

        public Car Car { get; set; }

        public Customer Customer { get; set; }

        public User User { get; set; }

        public Boolean Archived { get; set; }

        // Service
        public ICollection<Service> Services { get; set; }

        protected Invoice()
        {

        }
    }
}
