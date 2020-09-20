using Data.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDataContext context;
        public ServiceRepository(AppDataContext context)
        {
            this.context = context;
        }

        public async Task<Service> Add(Service service)
        {
            await context.Services.AddAsync(service);
            await context.SaveChangesAsync();
            return service;
        }

        public async Task<bool> Delete(long id)
        {
            var service = context.Services.FirstOrDefault(z => z.Id == id);
            if (service == null) return false;
            context.Services.Remove(service);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Service> GetById(long id)
        {
            return await context.Services.SingleOrDefaultAsync(z => z.Id == id);
        }

        public async Task<IEnumerable<Service>> GetByInvoiceId(long invoiceId)
        {
            return await context.Services.Where(z => z.InvoiceId == invoiceId).ToListAsync();
        }

        public async Task<Service> GetByName(string name)
        {
            return await context.Services.FirstOrDefaultAsync(z => z.ServiceName.Trim().Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Service> Update(Service service)
        {
            var change = context.Services.Attach(service);
            change.State = EntityState.Modified;
            await context.SaveChangesAsync();
            return service;
        }
    }
}
