﻿using Data.Domain.Common;
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
            car.CarNo = car.CarNo.CleanText().ToUpper();
            await context.Cars.AddAsync(car);
            await context.SaveChangesAsync();
            return car;
        }

        public async Task<bool> Delete(string carNo)
        {
            var car = context.Cars.FirstOrDefault(z => z.CarNo == carNo);
            if (car == null) return false;
            context.Cars.Remove(car);
            await context.SaveChangesAsync();
            return true;
        }


        public async Task<Car> GetByCarNo(string carNo)
        {
            return await context.Cars.FirstOrDefaultAsync(z => z.CarNo.ToUpper().Equals(carNo.CleanText().ToUpper()));
        }

        public async Task<Car> Update(Car car)
        {
            car.CarNo = car.CarNo.CleanText().ToUpper();

            var local = await context.Set<Car>().FirstOrDefaultAsync(z => z.CarNo == car.CarNo);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }

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

        public async Task<IEnumerable<Car>> FindByCarNoPaged(string carNo, PaginationFilter pagination)
        {
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;

            return await context.Cars.Where(z => z.CarNo.ToUpper().Contains(carNo.CleanText().ToUpper()))
                .Skip(skip).Take(pagination.PageSize)
                .ToListAsync();
        }

        public async Task<long> GetCount()
        {
            return await context.Cars.CountAsync();
        }
    }
}
