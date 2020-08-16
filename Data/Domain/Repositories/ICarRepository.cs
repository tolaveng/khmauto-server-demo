using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface ICarRepository
    {
        public Task<IEnumerable<Car>> GetAll();
        public Task<IEnumerable<Car>> FindByCarNo(string carNo);
        public Task<Car> GetById(long id);
        public Task<Car> GetByCarNo(string carNo);
        public Task<Car> Add(Car car);
        public Task<Car> Update(Car car);
        public Task<bool> Delete(long id);
        public Task<long> GetCount();
    }
}
