using Data.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDataContext context;
        public TransactionRepository(AppDataContext context)
        {
            this.context = context;
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await context.Database.BeginTransactionAsync();
        }
    }
}
