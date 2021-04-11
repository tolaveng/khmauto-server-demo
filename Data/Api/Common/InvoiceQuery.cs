using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Api.Common
{
    public class InvoiceQuery
    {
        public long InvoiceNo { get; set; }
        public string CarNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Customer { get; set; }
    }
}
