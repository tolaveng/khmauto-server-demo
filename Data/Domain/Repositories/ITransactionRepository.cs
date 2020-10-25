using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface ITransactionRepository
    {
        public Task<IDbContextTransaction> BeginTransaction();
        public Task CommitTransaction();
        public Task RollbackTransaction();
        public Task DisposeTransaction();
    }
}
