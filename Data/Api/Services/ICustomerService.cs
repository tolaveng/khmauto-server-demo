using Data.Api.Common;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface ICustomerService
    {
        Task<PaginationResponse<CustomerDto>> GetAllPaged(PaginationQuery pagination);
        Task<CustomerDto> GetById(long id);
        Task<CustomerDto> GetByPhoneNo(string phoneNo);
        Task<CustomerDto> GetByName(string name);
        Task Add(CustomerDto customer);
        Task Delete(long id);
        Task Update(CustomerDto customer);
        Task<CustomerDto> CreateOrUpdate(CustomerDto customer);
    }
}
