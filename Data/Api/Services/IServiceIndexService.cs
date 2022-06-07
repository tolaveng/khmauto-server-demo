using Data.Api.Common;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public interface IServiceIndexService
    {
        Task<IEnumerable<ServiceIndexDto>> FindByServiceName(string serviceName, int limit);
        Task<PaginationResponse<ServiceIndexDto>> FindByServiceNamePaged(string serviceName, PaginationQuery pagination);
        Task<IEnumerable<ServiceIndexDto>> GetAll(int limit);
        Task Update(int id, string serviceName);
        Task DeleteById(int id);
    }
}
