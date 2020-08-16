using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Common
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationFilter() { }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
