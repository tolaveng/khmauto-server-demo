using AutoMapper;
using Data.Api.Common;
using Data.Domain.Common;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.DTO;
using Data.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _repository;
        private readonly IMapper _mapper;

        public CarService(ICarRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        public async Task Add(CarDto car)
        {
            var insert = _mapper.Map<Car>(car);
            await _repository.Add(insert);
        }

        public async Task Delete(string carNo)
        {
            await _repository.Delete(carNo);
        }

        public async Task<PaginationResponse<CarDto>> GetAllPaged(PaginationQuery pagination)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(pagination);
            var totalCount = await _repository.GetCount();
            var cars = await _repository.GetAllPaged(paginationFilter);
            var data = _mapper.Map<List<CarDto>>(cars);
            var hasNext = (paginationFilter.PageNumber * paginationFilter.PageSize) < totalCount;
            return new PaginationResponse<CarDto>(data, totalCount, hasNext, pagination);
        }


        public async Task<CarDto> GetByCarNo(string carNo)
        {
            var car = await _repository.GetByCarNo(carNo);
            return _mapper.Map<CarDto>(car);
        }

        public async Task<PaginationResponse<CarDto>> FindByCarNoPaged(string carNo, PaginationQuery pagination)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(pagination);
            var cars = await _repository.FindByCarNoPaged(carNo, paginationFilter);
            var totalCount = await _repository.GetCount();
            var data = _mapper.Map<List<CarDto>>(cars);
            var hasNext = (paginationFilter.PageNumber * paginationFilter.PageSize) < totalCount;
            return new PaginationResponse<CarDto>(data, totalCount, hasNext, pagination);
        }

        public async Task Update(CarDto car)
        {
            var update = _mapper.Map<Car>(car);
            await _repository.Update(update);
        }

        public async Task<CarDto> CreateOrUpdate(CarDto car)
        {
            if (string.IsNullOrWhiteSpace(car.CarNo))
                throw new ArgumentException("Car No is empty");
            
           var update = await _repository.GetByCarNo(car.CarNo);
           if (update == null)
            {
                var insert = _mapper.Map<Car>(car);
                var newCar = await _repository.Add(insert);
                return _mapper.Map<CarDto>(newCar);
            }

            update = _mapper.Map<Car>(car);
            var updated = await _repository.Update(update);
            return _mapper.Map<CarDto>(updated);
            
        }
    }
}
