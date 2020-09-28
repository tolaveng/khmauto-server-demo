using Data.Api.Common;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public interface IInvoiceService
    {
        Task<PagedResponse<InvoiceDto>> GetAllPaged(PaginationQuery pagination);
        Task<PagedResponse<InvoiceDto>> GetByQuery(PaginationQuery pagination, InvoiceQuery query);
        Task Add(InvoiceDto invoice);
        Task Archive(long id);
        Task Update(InvoiceDto invoice);
    }
}
