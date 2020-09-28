using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public interface IServiceIndexService
    {
        Task<IEnumerable<ServiceIndexDto>> FindByServiceName(string serviceName);
    }
}
