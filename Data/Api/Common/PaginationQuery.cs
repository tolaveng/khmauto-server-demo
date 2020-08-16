using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Api.Common
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationQuery() { }

        public PaginationQuery(int pageNumber, int pageSize) {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
