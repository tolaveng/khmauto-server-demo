using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Models
{
    public class Quote
    {
        public long QuoteId { get; set; }
        public DateTime QuoteDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        
        public float Gst { get; set; }
        public string Note { get; set; }

        public string CarNo { get; set; }
        public virtual Car Car { get; set; }

        public string FullName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Abn { get; set; }

        public int UserId { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public Quote() {
            this.Services = new HashSet<Service>();
        }
    }
}
