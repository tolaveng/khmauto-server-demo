using Data.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public class ServiceIndexRepository : IServiceIndexRepository
    {
        private readonly AppDataContext context;
        public ServiceIndexRepository(AppDataContext context)
        {
            this.context = context;
        }

        public async Task AddOrUpdateService(string serviceName, decimal price)
        {
            var service = await GetService(serviceName);
            if (service != null)
            {
                service.ServicePrice = price;
                context.ServiceIndexs.Update(service);
            } else
            {
                service = new ServiceIndex() {
                    ServiceName = serviceName.Trim(),
                    ServicePrice = price
                };
                await context.ServiceIndexs.AddAsync(service);
            }
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ServiceIndex>> FindByServiceName(string serviceName)
        {
            return await context.ServiceIndexs.Where(z => z.ServiceName.Contains(serviceName.Trim())).ToListAsync();
        }

        public async Task<ServiceIndex> GetService(string serviceName)
        {
            return await context.ServiceIndexs.SingleOrDefaultAsync(z => z.ServiceName.Equals(serviceName.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
