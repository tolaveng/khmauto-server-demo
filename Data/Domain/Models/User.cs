using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Domain.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool isAdmin { get; set; }


        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Quote> Quotes { get; set; }

        protected User() { }

    }
}
