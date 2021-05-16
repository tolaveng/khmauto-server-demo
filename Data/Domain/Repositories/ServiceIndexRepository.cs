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
            try
            {
                var service = await GetService(serviceName);
                if (service != null)
                {
                    service.ServicePrice = price;
                    context.ServiceIndexs.Update(service);
                }
                else
                {
                    service = new ServiceIndex()
                    {
                        ServiceName = serviceName.Trim(),
                        ServicePrice = price
                    };
                    await context.ServiceIndexs.AddAsync(service);
                }
                await context.SaveChangesAsync();

            } catch(Exception)
            {
                //ignored
            }
        }

        public async Task<IEnumerable<ServiceIndex>> FindByServiceName(string serviceName)
        {
            return await context.ServiceIndexs.Where(z => z.ServiceName.Contains(serviceName.Trim(), StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public async Task<ServiceIndex> GetService(string serviceName)
        {
            return await context.ServiceIndexs.SingleOrDefaultAsync(z => z.ServiceName.Equals(serviceName.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<ServiceIndex>> GetAll(int limit)
        {
            try
            {
                // Distinct by using group by
                return await context.ServiceIndexs.Where(x => !string.IsNullOrWhiteSpace(x.ServiceName))
                    .GroupBy(x => new { x.ServiceName, x.ServicePrice })
                    .Select(g => new ServiceIndex() { ServiceName = g.Key.ServiceName, ServicePrice = g.Key.ServicePrice })
                    .Take(limit)
                    .ToListAsync();
            }
            catch(Exception e)
            {
                return new List<ServiceIndex>();
            }
        }
    }
}
