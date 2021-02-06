using Data.Api.Common;
using Data.Domain.Common;
using Data.Domain.Models;
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
                    .Include(z => z.Customer)
                    .Include(z => z.Car)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(z => z.InvoiceId == id);
        }

        public async Task<Invoice> GetByNo(long no)
        {
            return await context.Invoices
                .Include(z => z.Services)
                .Include(z => z.Customer)
                .Include(z => z.Car)
                .AsNoTracking()
                .SingleOrDefaultAsync(z => z.InvoiceNo == no);
        }

        public async Task<Invoice> Update(Invoice invoice)
        {
            var change = context.Invoices.Attach(invoice);
            change.State = EntityState.Modified;
            await context.SaveChangesAsync();
            return invoice;
        }

        public async Task<long> GetCount()
        {
            return await context.Invoices.CountAsync();
        }

        public async Task<IEnumerable<Invoice>> GetByCarId(long carId)
        {
            return await context.Invoices.Where(z => z.Car.CarId == carId).ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetAllPaged(PaginationFilter pagination)
        {
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;
            return await context.Invoices.Where(z => !z.Archived).OrderByDescending(z => z.InvoiceId)
                .Skip(skip).Take(pagination.PageSize).ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetByQuery(PaginationFilter pagination, InvoiceQuery query)
        {
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;
            return await context.Invoices.Where(z => !z.Archived &&
                (string.IsNullOrWhiteSpace(query.PlateNo) || z.Car.PlateNo.Contains(query.PlateNo)) &&
                (query.DateTime == null || z.InvoiceDateTime.Date.Equals(query.DateTime.Date)) &&
                (string.IsNullOrWhiteSpace(query.CustomerName) || z.Customer.FullName.Contains(query.CustomerName)) &&
                (string.IsNullOrWhiteSpace(query.CustomerPhone) || z.Customer.Phone.Contains(query.CustomerPhone))
            )
                .OrderByDescending(z => z.CarId)
                .Skip(skip).Take(pagination.PageSize).ToListAsync();
        }

        
        public async Task<long> GetCountByQuery(InvoiceQuery query)
        {
            return await context.Invoices.Where(z => !z.Archived &&
                (string.IsNullOrWhiteSpace(query.PlateNo) || z.Car.PlateNo.Contains(query.PlateNo)) &&
                (query.DateTime == null || z.InvoiceDateTime.Date.Equals(query.DateTime.Date)) &&
                (string.IsNullOrWhiteSpace(query.CustomerName) || z.Customer.FullName.Contains(query.CustomerName)) &&
                (string.IsNullOrWhiteSpace(query.CustomerPhone) || z.Customer.Phone.Contains(query.CustomerPhone))
            ).CountAsync();
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
