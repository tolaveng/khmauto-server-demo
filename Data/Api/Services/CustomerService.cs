using AutoMapper;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        public async Task Add(CustomerDto customer)
        {
            var insert = _mapper.Map<Customer>(customer);
            await _repository.Add(insert);
        }

        public async Task Delete(long id)
        {
            await _repository.Delete(id);
        }

        public async Task<IEnumerable<CustomerDto>> GetAll()
        {
            var customer = await _repository.GetAll();
            return _mapper.Map<List<CustomerDto>>(customer);
        }

        public async Task<CustomerDto> GetById(long id)
        {
            var customer = await _repository.GetById(id);
            return _mapper.Map<CustomerDto>( customer);
        }

        public async Task<CustomerDto> GetByPhoneNo(string phoneNo)
        {
            var customer = await _repository.GetByPhone(phoneNo);
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> GetByName(string name)
        {
            var customer = await _repository.GetByFullName(name);
            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
