using AutoMapper;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        public async Task Create(CompanyDto company)
        {
            var newCompany = _mapper.Map<Company>(company);
            await _repository.Add(newCompany);
        }

        public async Task<CompanyDto> Get()
        {
            var companies = await _repository.GetAll();
            var company = companies.FirstOrDefault();
            if (company != null)
            {
                return _mapper.Map<CompanyDto>(company);
            }
            return new CompanyDto();
        }

        public async Task Update(CompanyDto company)
        {
            var update = _mapper.Map<Company>(company);
            await _repository.Update(update);
        }
    }
}
