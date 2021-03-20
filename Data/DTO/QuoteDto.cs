using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class QuoteDto
    {
        public long QuoteId { get; set; }
        public DateTime QuoteDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        public float Gst { get; set; }
        public string Note { get; set; }

        public long CarId { get; set; }
        public CarDto Car { get; set; }

        public string FullName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Abn { get; set; }

        public int UserId { get; set; }

        // Service
        public ICollection<ServiceDto> Services { get; set; }
        public QuoteDto() {
            this.Services = new HashSet<ServiceDto>();
        }
    }
}
