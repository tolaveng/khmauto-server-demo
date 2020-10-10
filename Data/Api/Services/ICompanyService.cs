using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public interface ICompanyService
    {
        Task<CompanyDto> Get();
        Task UpdateCompany(CompanyDto company);
        Task CreateCompany(CompanyDto company);
    }
}
