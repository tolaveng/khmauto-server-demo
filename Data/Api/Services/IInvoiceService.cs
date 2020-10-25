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
        Task<InvoiceDto> GetById(long id);
        Task<PagedResponse<InvoiceDto>> GetAllPaged(PaginationQuery pagination);
        Task<PagedResponse<InvoiceDto>> GetByQuery(PaginationQuery pagination, InvoiceQuery query);
        Task Create(InvoiceDto invoice);
        Task Archive(long id);
        Task Update(InvoiceDto invoice);
    }
}
