using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Api.Common
{
    public class InvoiceQuery
    {
        public string PlateNo { get; set; }
        public DateTime DateTime { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
    }
}
