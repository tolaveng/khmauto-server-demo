using Data.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<Quote> GetById(long id)
        {
            return await context.Quotes.SingleOrDefaultAsync(z => z.Id == id);
        }

        public async Task<Quote> Update(Quote quote)
        {
            var change = context.Quotes.Attach(quote);
            change.State = EntityState.Modified;
            await context.SaveChangesAsync();
            return quote;
        }
    }
}
