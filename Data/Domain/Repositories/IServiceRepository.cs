using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface IServiceRepository
    {
        public Task<Service> GetById(long id);
        public Task<Service> GetByName(string name);
        public Task<Service> Add(Service service);
        public Task<Service> Update(Service service);
        public Task<bool> Delete(long id);
    }
}
