using Data.Api.Common;
using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface IInvoiceRepository
    {
        public Task<IEnumerable<Invoice>> GetAllPaged(PaginationQuery pagination);
        public Task<IEnumerable<Invoice>> GetByQuery(PaginationQuery pagination, InvoiceQuery query);
        public Task<Invoice> GetById(long id);
        public Task<Invoice> GetByNo(long id);
        public Task<Invoice> Add(Invoice invoice);
        public Task<Invoice> Update(Invoice invoice);
        public Task<bool> Archive(long id);
        public Task<long> GetCount();
        public Task<long> GetMaxInvoiceNo();
        public Task<long> GetCountByQuery(InvoiceQuery query);
        public Task<IEnumerable<Invoice>> GetByCarNo(string carNo);
    }
}
