using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface IServiceIndex
    {
        public Task<ServiceIndex> GetService(string serviceName);
        public Task<IEnumerable<ServiceIndex>> FindByServiceName(string serviceName);
        public Task AddOrUpdateService(string serviceName, decimal price);
    }
}
