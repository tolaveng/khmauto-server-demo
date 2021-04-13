using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHMAuto.Responses
{
    public class SummaryReport
    {
        public long InvoiceNo {get; set; }
        public string InvoiceDate { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal Gst { get; set; }
    }
}
