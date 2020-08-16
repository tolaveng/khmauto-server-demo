﻿using System;
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

        public Car Car { get; set; }

        public Customer Customer { get; set; }

        public User User { get; set; }

        public ICollection<Service> Services { get; set; }

        protected Quote() {
        }
    }
}
