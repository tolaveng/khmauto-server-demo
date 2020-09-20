using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface IInvoiceRepository
    {
        public Task<Invoice> GetById(long id);
        public Task<Invoice> Add(Invoice invoice);
        public Task<Invoice> Update(Invoice invoice);
        public Task<bool> Archive(long id);
        public Task<long> GetCount();
        public Task<IEnumerable<Invoice>> GetByCarId(long carId);
    }
}
