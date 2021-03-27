using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceDto>> GetByInvoiceId(long invoicId);
        Task<IEnumerable<ServiceDto>> GetByCarNo(string carNo);
        Task<ServiceDto> GetById(long id);
        Task<ServiceDto> GetByName(string name);
        Task Add(ServiceDto service);
        Task Update(ServiceDto service);
        Task<bool> Delete(long id);
    }
}
