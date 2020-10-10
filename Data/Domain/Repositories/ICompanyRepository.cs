using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface ICompanyRepository
    {
        public Task<IEnumerable<Company>> GetAll();
        public Task<Company> GetById(int id);
        public Task Update(Company company);
        public Task Add(Company company);
    }
}
