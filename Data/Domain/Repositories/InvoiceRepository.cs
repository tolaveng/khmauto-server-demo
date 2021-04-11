using Data.Api.Common;
using Data.Domain.Common;
using Data.Domain.Models;
using Data.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDataContext context;
        public InvoiceRepository(AppDataContext context)
        {
            this.context = context;
        }

        public async Task<Invoice> Add(Invoice invoice)
        {
            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();
            return invoice;
        }

        public async Task<bool> Archive(long id)
        {
            var invoice = await GetById(id);
            if (invoice != null)
            {
                invoice.Archived = true;
                await Update(invoice);
                return true;
            }
            return false;
        }

        public async Task<Invoice> GetById(long id)
        {
            return await context.Invoices
                    .Include(z => z.Services)
                    .Include(z => z.Car)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(z => z.InvoiceId == id);
        }

        public async Task<Invoice> GetByNo(long no)
        {
            return await context.Invoices
                .Include(z => z.Services)
                .Include(z => z.Car)
                .AsNoTracking()
                .SingleOrDefaultAsync(z => z.InvoiceNo == no);
        }

        public async Task<Invoice> Update(Invoice invoice)
        {
            var change = context.Invoices.Attach(invoice);
            change.State = EntityState.Modified;

            // update service
            foreach (var service in invoice.Services)
            {
                if (service.ServiceId > 0)
                {
                    context.Entry(service).State = EntityState.Modified;
                }
            }

            // delete service
            var serviceIds = invoice.Services.Select(x => x.ServiceId).ToArray();
            var deleteServices = context.Services.Where(x => x.InvoiceId == invoice.InvoiceId && !serviceIds.Contains(x.ServiceId));
            context.Services.RemoveRange(deleteServices);

            await context.SaveChangesAsync();
            return invoice;
        }

        public async Task<long> GetCount()
        {
            return await context.Invoices.CountAsync();
        }

        public async Task<IEnumerable<Invoice>> GetByCarNo(string carNo)
        {
            return await context.Invoices.Where(z => z.Car.CarNo == carNo).ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetAllPaged(PaginationFilter pagination)
        {
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;
            return await context.Invoices.Where(z => !z.Archived)
                .Include(z => z.Car)
                .AsNoTracking()
                .OrderByDescending(z => z.InvoiceNo)
                .Skip(skip).Take(pagination.PageSize).ToListAsync();
        }

        public IQueryable<Invoice> GetQueryable(InvoiceQuery query)
        {
            var queryable = context.Invoices.Include(z => z.Car).AsNoTracking().AsQueryable();
            queryable = queryable.Where(z => !z.Archived);

            if (!string.IsNullOrWhiteSpace(query.CarNo))
            {
                queryable = queryable.Where(z => z.Car.CarNo.Contains(query.CarNo));
            }

            if (query.InvoiceNo != 0)
            {
                queryable = queryable.Where(z => z.InvoiceNo == query.InvoiceNo);
            }

            if (query.InvoiceDate != null && query.InvoiceDate != default)
            {
                // make time to mid night 23.59 PM
                if (query.InvoiceDate.TimeOfDay == TimeSpan.Zero)
                {
                    query.InvoiceDate = query.InvoiceDate.AddDays(1).AddMinutes(-1);
                }
                queryable = queryable.Where(z => z.InvoiceDate <= query.InvoiceDate);
            }

            if (!string.IsNullOrWhiteSpace(query.Customer))
            {
                queryable = queryable.Where(z => 
                    z.FullName.Contains(query.Customer, StringComparison.OrdinalIgnoreCase) ||
                    z.Phone.Contains(query.Customer)
                );
            }

            return queryable;
        }

        public async Task<IEnumerable<Invoice>> GetByQuery(PaginationFilter pagination, InvoiceQuery query)
        {
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;
            return await GetQueryable(query).OrderByDescending(z => z.InvoiceNo).Skip(skip).Take(pagination.PageSize).ToListAsync();
        }

        
        public async Task<long> GetCountByQuery(InvoiceQuery query)
        {
            return await GetQueryable(query).CountAsync();
        }

        public async Task<long> GetMaxInvoiceNo()
        {
            try
            {
                return await context.Invoices.MaxAsync(z => z.InvoiceNo);
            }
            catch(Exception)
            {
                return 0;
            }
        }
    }
}
