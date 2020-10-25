using Data.Domain.Common;
using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface ICarRepository
    {
        public Task<IEnumerable<Car>> GetAllPaged(PaginationFilter pagination);
        public Task<IEnumerable<Car>> FindByPlateNoPaged(string PlateNo, PaginationFilter pagination);
        public Task<Car> GetById(long id);
        public Task<Car> GetByPlateNo(string PlateNo);
        public Task<Car> Add(Car car);
        public Task<Car> Update(Car car);
        public Task<bool> Delete(long id);
        public Task<long> GetCount();
    }
}
