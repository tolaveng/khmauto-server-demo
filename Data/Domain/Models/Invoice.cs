using Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Models
{
    public class Invoice
    {
        public long InvoiceId { get; set; }
        public long InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaidDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public float Gst { get; set; }
        public string Note { get; set; }
        public long ODO { get; set; }

        public string CarNo { get; set; }
        public virtual Car Car { get; set; }

        public string FullName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Abn { get; set; }

        public int UserId { get; set; }

        public Boolean Archived { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public Invoice()
        {
            this.Services = new HashSet<Service>();
        }
    }
}
