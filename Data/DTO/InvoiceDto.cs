using Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class InvoiceDto
    {
        public long InvoiceId { get; set; }
        public long InvoiceNo { get; set; }
        public DateTime InvoiceDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaidDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public float Gst { get; set; }
        public string Note { get; set; }
        public long ODO { get; set; }

        public long CarId { get; set; }
        public CarDto Car { get; set; }

        public long CustomerId { get; set; }
        public CustomerDto Customer { get; set; }

        public int UserId { get; set; }

        public Boolean Archived { get; set; }

        // Service
        public ICollection<ServiceDto> Services { get; set; }

        public InvoiceDto() {
            Gst = 0.1f;
            Archived = false;
            this.Services = new HashSet<ServiceDto>();
        }
    }
}
