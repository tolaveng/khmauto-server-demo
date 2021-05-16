using Data.Api.Common;
using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface IQuoteRepository
    {
        public Task<IEnumerable<Quote>> GetAllPaged(PaginationQuery pagination);
        public Task<IEnumerable<Quote>> GetByQuery(PaginationQuery pagination, QuoteQuery query);
        public Task<Quote> GetById(long id);
        public Task<Quote> Add(Quote quote);
        public Task<Quote> Update(Quote quote);
        public Task<bool> Delete(long id);
        public Task<long> GetCount();
        public Task<long> GetCountByQuery(QuoteQuery query);
    }
}
