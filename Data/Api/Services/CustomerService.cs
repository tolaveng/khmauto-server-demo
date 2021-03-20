using AutoMapper;
using Data.Api.Common;
using Data.Domain.Common;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.DTO;
using Data.Utils;
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

        public async Task<PaginationResponse<CustomerDto>> GetAllPaged(PaginationQuery pagination)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(pagination);
            var totalCount = await _repository.GetCount();
            var customers = await _repository.GetAllPaged(paginationFilter);
            var data = _mapper.Map<List<CustomerDto>>(customers);
            var hasNext = (paginationFilter.PageNumber * paginationFilter.PageSize) < totalCount;
            return new PaginationResponse<CustomerDto>(data, totalCount, hasNext, pagination);
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

        public async Task Update(CustomerDto customer)
        {
            var update = _mapper.Map<Customer>(customer);
            await _repository.Update(update);
        }

        public async Task<CustomerDto> CreateOrUpdate(CustomerDto customer)
        {
            if (customer.CustomerId != 0)
            {
                var update = _mapper.Map<Customer>(customer);
                update.Phone = update.Phone.Replace(" ", "").Trim();
                var updatedCustomer = await _repository.Update(update);
                return _mapper.Map<CustomerDto>(updatedCustomer);
            }
            else
            {
                // check by phone number
                Customer toUpdate = null;
                if (!string.IsNullOrWhiteSpace(customer.Phone))
                {
                    toUpdate = await _repository.GetByPhone(customer.Phone);
                }
                if (toUpdate == null && !string.IsNullOrWhiteSpace(customer.Email))
                {
                    toUpdate = await _repository.GetByEmail(customer.Email);
                }

                if (toUpdate != null)
                {
                    toUpdate.FullName = customer.FullName;
                    toUpdate.Company = customer.Company;
                    toUpdate.Abn = customer.Abn;
                    toUpdate.Address = customer.Address;
                    toUpdate.Email = customer.Email;
                    toUpdate.Phone = customer.Phone.CleanText();
                    var updatedCustomer = await _repository.Update(toUpdate);
                    return _mapper.Map<CustomerDto>(updatedCustomer);

                } else
                {
                    var insert = _mapper.Map<Customer>(customer);
                    var newCustomer = await _repository.Add(insert);
                    newCustomer.Phone = newCustomer.Phone.CleanText();
                    return _mapper.Map<CustomerDto>(newCustomer);
                }

            }
        }
    }
}
