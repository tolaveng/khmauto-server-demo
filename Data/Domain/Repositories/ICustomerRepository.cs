﻿using Data.Domain.Common;
using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface ICustomerRepository
    {
        public Task<IEnumerable<Customer>> GetAllPaged(PaginationFilter pagination);
        public Task<Customer> GetById(long id);
        public Task<Customer> GetByFullName(string name);
        public Task<Customer> GetByPhone(string phone);
        public Task<Customer> GetByEmail(string email);
        public Task<Customer> Add(Customer customer);
        public Task<Customer> Update(Customer customer);
        public Task<bool> Delete(long id);
        public Task<long> GetCount();
    }
}
