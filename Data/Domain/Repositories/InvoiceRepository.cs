using Data.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public class InvoiceRepository : IInvoice
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
            return await context.Invoices.SingleOrDefaultAsync(z => z.Id == id);
        }

        public async Task<Invoice> Update(Invoice invoice)
        {
            var change = context.Invoices.Attach(invoice);
            change.State = EntityState.Modified;
            await context.SaveChangesAsync();
            return invoice;
        }
    }
}
