using Data.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDataContext context;
        public CompanyRepository(AppDataContext context)
        {
            this.context = context;
        }

        public async Task Add(Company company)
        {
            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
        }

        public async Task<Company> GetById(int id)
        {
            return await context.Companies.SingleOrDefaultAsync( z => z.Id == id);
        }

        public async Task<IEnumerable<Company>> GetAll()
        {
            return await context.Companies.ToListAsync();
        }

        public async Task Update(Company company)
        {
            var change = context.Companies.Attach(company);
            change.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

    }
}
