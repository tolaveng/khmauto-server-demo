using Data.Domain.Common;
using Data.Domain.Models;
using Data.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly AppDataContext context;
        public CarRepository(AppDataContext context)
        {
            this.context = context;
        }

        public async Task<Car> Add(Car car)
        {
            car.PlateNo = car.PlateNo.CleanText().ToUpper();
            await context.Cars.AddAsync(car);
            await context.SaveChangesAsync();
            return car;
        }

        public async Task<bool> Delete(long id)
        {
            var car = context.Cars.FirstOrDefault(z => z.CarId == id);
            if (car == null) return false;
            context.Cars.Remove(car);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Car> GetById(long id)
        {
            return await context.Cars.FindAsync(id);
        }

        public async Task<Car> GetByPlateNo(string PlateNo)
        {
            return await context.Cars.FirstOrDefaultAsync(z => z.PlateNo.ToUpper().Equals(PlateNo.CleanText().ToUpper()));
        }

        public async Task<Car> Update(Car car)
        {
            car.PlateNo = car.PlateNo.CleanText().ToUpper();
            var change = context.Cars.Attach(car);
            change.State = EntityState.Modified;
            await context.SaveChangesAsync();
            return car;
        }

        public async Task<IEnumerable<Car>> GetAllPaged(PaginationFilter pagination)
        {
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;
            return await context.Cars.Skip(skip).Take(pagination.PageSize).ToListAsync();
        }

        public async Task<IEnumerable<Car>> FindByPlateNoPaged(string PlateNo, PaginationFilter pagination)
        {
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;

            return await context.Cars.Where(z => z.PlateNo.ToUpper().Contains(PlateNo.CleanText().ToUpper()))
                .Skip(skip).Take(pagination.PageSize)
                .ToListAsync();
        }

        public async Task<long> GetCount()
        {
            return await context.Cars.CountAsync();
        }
    }
}
