using Data.Api.Common;
using Data.Domain.Common;
using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface IQuoteRepository
    {
        public Task<IEnumerable<Quote>> GetAllPaged(PaginationFilter pagination);
        public Task<IEnumerable<Quote>> GetByQuery(PaginationFilter pagination, InvoiceQuery query);
        public Task<Quote> GetById(long id);
        public Task<Quote> Add(Quote quote);
        public Task<Quote> Update(Quote quote);
        public Task<bool> Delete(long id);
        public Task<long> GetCount();
        public Task<long> GetCountByQuery(InvoiceQuery query);
    }
}
