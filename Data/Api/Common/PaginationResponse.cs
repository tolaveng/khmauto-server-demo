using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Api.Common
{
    public class PaginationResponse<T>
    {

        public IEnumerable<T> Data { get; set; }
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public long TotalCount { get; set; }

        public bool HasNext { get; set; }

        public PaginationResponse()
        {
        }

        public PaginationResponse(IEnumerable<T> data, long totalCount, bool hasNext, int pageNumber, int pageSize)
        {
            Data = data;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            HasNext = hasNext;
        }

        public PaginationResponse(IEnumerable<T> data, long totalCount, bool hasNext, PaginationQuery pagination)
        {
            Data = data;
            TotalCount = totalCount;
            PageNumber = pagination.PageNumber;
            PageSize = pagination.PageSize;
            HasNext = hasNext;
        }
    }
}
