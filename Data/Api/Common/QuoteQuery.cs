﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Api.Common
{
    public class QuoteQuery
    {
        public long QuoteId { get; set; }
        public string CarNo { get; set; }
        public DateTime QuoteDate { get; set; }
        public string Customer { get; set; }

        public string SortBy { get; set; }
        public string SortDir { get; set; }
    }
}
