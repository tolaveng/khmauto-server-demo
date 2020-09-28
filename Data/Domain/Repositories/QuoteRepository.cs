using Data.Api.Common;
using Data.Domain.Common;
using Data.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly AppDataContext context;
        public QuoteRepository(AppDataContext context)
        {
            this.context = context;
        }

        public async Task<Quote> Add(Quote quote)
        {
            context.Quotes.Add(quote);
            await context.SaveChangesAsync();
            return quote;
        }

        public async Task<bool> Delete(long id)
        {
            var quote = await GetById(id);
            if (quote != null)
            {
                context.Quotes.Remove(quote);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Quote>> GetAllPaged(PaginationFilter pagination)
        {
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;
            return await context.Quotes.OrderByDescending(z => z.Id)
                .Skip(skip).Take(pagination.PageSize).ToListAsync();
        }

        public async Task<IEnumerable<Quote>> GetByQuery(PaginationFilter pagination, InvoiceQuery query)
        {
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;
            return await context.Quotes.Where(z => z.Id > 0 &&
                (string.IsNullOrWhiteSpace(query.PlateNo) || z.Car.CarNo.Contains(query.PlateNo)) &&
                (query.DateTime == null || z.QuoteDateTime.Date.Equals(query.DateTime.Date)) &&
                (string.IsNullOrWhiteSpace(query.CustomerName) || z.Customer.FullName.Contains(query.CustomerName)) &&
                (string.IsNullOrWhiteSpace(query.CustomerPhone) || z.Customer.Phone.Contains(query.CustomerPhone))
            )
                .OrderByDescending(z => z.Id)
                .Skip(skip).Take(pagination.PageSize).ToListAsync();
        }

        public async Task<Quote> GetById(long id)
        {
            return await context.Quotes.SingleOrDefaultAsync(z => z.Id == id);
        }

        public async Task<long> GetCount()
        {
            return await context.Quotes.CountAsync();
        }

        public async Task<Quote> Update(Quote quote)
        {
            var change = context.Quotes.Attach(quote);
            change.State = EntityState.Modified;
            await context.SaveChangesAsync();
            return quote;
        }

        public async Task<long> GetCountByQuery(InvoiceQuery query)
        {
            return await context.Quotes.Where(z => z.Id > 0 &&
                (string.IsNullOrWhiteSpace(query.PlateNo) || z.Car.CarNo.Contains(query.PlateNo)) &&
                (query.DateTime == null || z.QuoteDateTime.Date.Equals(query.DateTime.Date)) &&
                (string.IsNullOrWhiteSpace(query.CustomerName) || z.Customer.FullName.Contains(query.CustomerName)) &&
                (string.IsNullOrWhiteSpace(query.CustomerPhone) || z.Customer.Phone.Contains(query.CustomerPhone))
            ).CountAsync();
        }
    }
}
