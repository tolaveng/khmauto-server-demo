using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHMAuto.Requests
{
    public class SummaryReportFilter
    {
        public string FromDate { get; set; }
   
        public string ToDate { get; set; }
        
        public string SortBy { get; set; }

        public string SortDir { get; set; }
   
    }
}
