using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Domain.Models
{
    public class ServiceIndex
    {
        [Key]
        public string ServiceName { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public Decimal ServicePrice { get; set; }

        public ServiceIndex()
        {

        }
    }
}
