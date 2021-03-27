﻿using Data.Domain.Common;
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
        public Task<IEnumerable<Car>> FindByCarNoPaged(string carNo, PaginationFilter pagination);
        public Task<Car> GetByCarNo(string carNo);
        public Task<Car> Add(Car car);
        public Task<Car> Update(Car car);
        public Task<bool> Delete(string carNo);
        public Task<long> GetCount();
    }
}
