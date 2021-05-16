using Data.Api.Common;
using Data.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Quote>> GetAllPaged(PaginationQuery pagination)
        {
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;
            return await context.Quotes
                .Include(z => z.Car)
                .AsNoTracking()
                .OrderByDescending(z => z.QuoteId)
                .Skip(skip).Take(pagination.PageSize).ToListAsync();
        }

        public IQueryable<Quote> GetQueryable(QuoteQuery query)
        {
            var queryable = context.Quotes.Include(z => z.Car).AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(query.CarNo))
            {
                queryable = queryable.Where(z => z.Car.CarNo.Contains(query.CarNo));
            }

            if (query.QuoteId != 0)
            {
                queryable = queryable.Where(z => z.QuoteId == query.QuoteId);
            }

            if (query.QuoteDate != null && query.QuoteDate != default)
            {
                // make time to mid night 23.59 PM
                if (query.QuoteDate.TimeOfDay == TimeSpan.Zero)
                {
                    query.QuoteDate = query.QuoteDate.AddDays(1).AddMinutes(-1);
                }
                queryable = queryable.Where(z => z.QuoteDate <= query.QuoteDate);
            }

            if (!string.IsNullOrWhiteSpace(query.Customer))
            {
                queryable = queryable.Where(z => z.FullName.Contains(query.Customer, StringComparison.OrdinalIgnoreCase) || z.Phone.Contains(query.Customer));
            }

            
            return queryable;
        }

        public async Task<IEnumerable<Quote>> GetByQuery(PaginationQuery pagination, QuoteQuery query)
        {
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;
            return await GetQueryable(query).OrderByDescending(z => z.QuoteId).Skip(skip).Take(pagination.PageSize).ToListAsync();
        }

        public async Task<Quote> GetById(long id)
        {
            return await context.Quotes
                .Include(z => z.Services)
                .Include(z => z.Car)
                .AsNoTracking()
                .SingleOrDefaultAsync(z => z.QuoteId == id);
        }

        public async Task<long> GetCount()
        {
            return await context.Quotes.CountAsync();
        }

        public async Task<Quote> Update(Quote quote)
        {
            var change = context.Quotes.Attach(quote);
            change.State = EntityState.Modified;

            // update service
            foreach (var service in quote.Services)
            {
                if (service.ServiceId > 0)
                {
                    context.Entry(service).State = EntityState.Modified;
                }
            }

            // delete service
            var serviceIds = quote.Services.Select(x => x.ServiceId).ToArray();
            var deleteServices = context.Services.Where(x => x.QuoteId == quote.QuoteId && !serviceIds.Contains(x.ServiceId));
            context.Services.RemoveRange(deleteServices);

            await context.SaveChangesAsync();
            return quote;
        }

        public async Task<long> GetCountByQuery(QuoteQuery query)
        {
            return await GetQueryable(query).CountAsync();
        }
    }
}
