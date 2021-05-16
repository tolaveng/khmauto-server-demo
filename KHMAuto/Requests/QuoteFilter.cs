using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHMAuto.Requests
{
    public class QuoteFilter
    {
        public string QuoteId { get; set; }
        public string CarNo { get; set; }
        public string Customer { get; set; }
        public string QuoteDate { get; set; }

        public string SortBy { get; set; }

        public string SortDir { get; set; }
    }
}
