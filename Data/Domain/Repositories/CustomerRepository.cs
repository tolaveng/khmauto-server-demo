using Data.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDataContext context;
        public CustomerRepository(AppDataContext context)
        {
            this.context = context;
        }

        public async Task<Customer> Add(Customer customer)
        {
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> Delete(long id)
        {
            var customer = context.Customers.FirstOrDefault(z => z.Id == id);
            if (customer == null) return false;
            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await context.Customers.ToListAsync();
        }

        public async Task<Customer> GetByEmail(string email)
        {
            return await context.Customers.FirstOrDefaultAsync(z => z.Email.Trim().Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Customer> GetByFullName(string name)
        {
            return await context.Customers.FirstOrDefaultAsync(z => z.FullName.Trim().Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Customer> GetById(long id)
        {
            return await context.Customers.SingleOrDefaultAsync(z => z.Id == id);
        }

        public async Task<Customer> GetByPhone(string phone)
        {
            return await context.Customers.FirstOrDefaultAsync(z => z.Phone.Trim().Equals(phone.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public async Task<long> GetCount()
        {
            return await context.Customers.CountAsync();
        }

        public async Task<Customer> Update(Customer customer)
        {
            var change = context.Customers.Attach(customer);
            change.State = EntityState.Modified;
            await context.SaveChangesAsync();
            return customer;
        }
    }
}
