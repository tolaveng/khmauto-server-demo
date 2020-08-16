using Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class InvoiceDto
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

        public CarDto Car { get; set; }

        public CustomerDto Customer { get; set; }

        public UserDto User { get; set; }

        public Boolean Archived { get; set; }

        // Service
        public ICollection<ServiceDto> Services { get; set; }
    }
}
