using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Api.Common
{
    public class PagedResponse<T>
    {

        public IEnumerable<T> Data { get; set; }
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public long TotalCount { get; set; }

        public PagedResponse()
        {
        }

        public PagedResponse(IEnumerable<T> data, long totalCount, int pageNumber, int pageSize)
        {
            Data = data;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public PagedResponse(IEnumerable<T> data, long totalCount, PaginationQuery pagination)
        {
            Data = data;
            TotalCount = totalCount;
            PageNumber = pagination.PageNumber;
            PageSize = pagination.PageSize;
        }
    }
}
