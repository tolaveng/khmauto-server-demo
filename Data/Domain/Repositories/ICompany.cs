using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface ICompany
    {
        public Task<Company> GetById(int id);
        public Task<Company> Update(Company company);
    }
}
