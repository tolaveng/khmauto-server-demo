using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class CompanyDto
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Abn { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public float Gst { get; set; }
        public string GstNumber { get; set; }
        public string BankName { get; set; }
        public string BankBsb { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }

        public CompanyDto() { }
    }
}
