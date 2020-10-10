using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class QuoteDto
    {
        public long Id { get; set; }
        public DateTime QuoteDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        public float Gst { get; set; }
        public string Note { get; set; }

        public CarDto Car { get; set; }

        public CustomerDto Customer { get; set; }

        public UserDto User { get; set; }

        // Service
        public ICollection<ServiceDto> Services { get; set; }
        public QuoteDto() { 
        }
    }
}
