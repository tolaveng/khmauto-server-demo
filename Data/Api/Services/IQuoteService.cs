using Data.Api.Common;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public interface IQuoteService
    {
        Task<PagedResponse<QuoteDto>> GetAllPaged(PaginationQuery pagination);
        Task<PagedResponse<QuoteDto>> GetByQuery(PaginationQuery pagination, InvoiceQuery query);
        Task Add(QuoteDto quote);
        Task Delete(long id);
        Task Update(QuoteDto quote);
    }
}
