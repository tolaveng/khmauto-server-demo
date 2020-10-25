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

        public async Task CommitTransaction()
        {
            context.Database.CommitTransaction();
        }

        public async Task DisposeTransaction()
        {
            context.Database.CurrentTransaction.Dispose();
        }

        public async Task RollbackTransaction()
        {
            context.Database.RollbackTransaction();
        }
    }
}
