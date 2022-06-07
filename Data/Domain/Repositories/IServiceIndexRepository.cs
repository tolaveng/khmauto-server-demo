using Data.Api.Common;
using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface IServiceIndexRepository
    {
        public Task<ServiceIndex> GetService(string serviceName);
        public Task<IEnumerable<ServiceIndex>> FindByServiceName(string serviceName, int limit);
        public Task<IEnumerable<ServiceIndex>> FindByServiceNamePaged(string serviceName, PaginationQuery pagination);
        public Task AddOrUpdateService(string serviceName, decimal price);
        public Task<IEnumerable<ServiceIndex>> GetAll(int limit);
        public Task UpdateService(int id, string serviceName);
        public Task DeleteService(int id);
    }
}
