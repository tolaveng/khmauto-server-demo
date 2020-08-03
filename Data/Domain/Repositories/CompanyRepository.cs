using Data.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<Company> GetById(int id)
        {
            return await context.Companies.FindAsync(id);
        }

        public async Task<Company> Update(Company company)
        {
            var change = context.Companies.Attach(company);
            change.State = EntityState.Modified;
            await context.SaveChangesAsync();
            return company;
        }
    }
}
