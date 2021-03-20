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

        public string FullName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Abn { get; set; }

        public int UserId { get; set; }

        public Boolean Archived { get; set; }

        // Service
        public ICollection<ServiceDto> Services { get; set; }

        public InvoiceDto() {
            Gst = 10;
            Archived = false;
            this.Services = new HashSet<ServiceDto>();
        }
    }
}
