﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Domain.Models
{
    public class Service
    {
        public long ServiceId { get; set; }
        public string ServiceName { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal ServicePrice { get; set; }
        public int ServiceQty { get; set; }

        public long? InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }

        public long? QuoteId { get; set; }
        public virtual Quote Quote { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal ServicePriceOld { get; set; }

        public Service() { }
    }
}
