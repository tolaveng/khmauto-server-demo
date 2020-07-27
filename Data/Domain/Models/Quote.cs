using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Models
{
    public class Quote
    {
        public long Id { get; set; }
        public DateTime QuoteDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        
        public float Gst { get; set; }
        public string Note { get; set; }

        // Car
        public long CarId { get; set; }
        public Car Car { get; set; }

        // Customer
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }

        // User
        public int UserId { get; set; }
        public User User { get; set; }

        // Service
        public ICollection<Service> Services { get; set; }

        protected Quote() {
        }
    }
}
