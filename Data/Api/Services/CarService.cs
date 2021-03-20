using AutoMapper;
using Data.Api.Common;
using Data.Domain.Common;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.DTO;
using Data.Utils;
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

        public async Task Delete(long id)
        {
            await _repository.Delete(id);
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

        public async Task<CarDto> GetById(long id)
        {
            var car = await _repository.GetById(id);
            return _mapper.Map<CarDto>(car);
        }

        public async Task<CarDto> GetByPlateNo(string PlateNo)
        {
            var car = await _repository.GetByPlateNo(PlateNo);
            return _mapper.Map<CarDto>(car);
        }

        public async Task<PaginationResponse<CarDto>> FindByPlateNoPaged(string PlateNo, PaginationQuery pagination)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(pagination);
            var cars = await _repository.FindByPlateNoPaged(PlateNo, paginationFilter);
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
            if (car.CarId != 0)
            {
                var update = _mapper.Map<Car>(car);
                var updated = await _repository.Update(update);
                return _mapper.Map<CarDto>(updated);
            }
            else
            {
                Car toUpdate = null;
                if (!string.IsNullOrWhiteSpace(car.PlateNo.CleanText()))
                {
                    toUpdate = await _repository.GetByPlateNo(car.PlateNo);
                }
                
                if (toUpdate != null)
                {
                    toUpdate.CarModel = car.CarModel.Trim();
                    toUpdate.PlateNo = car.PlateNo.Trim();
                    toUpdate.CarMake = car.CarMake;
                    toUpdate.CarYear = car.CarYear;
                    toUpdate.ODO = car.ODO;
                    
                    var updated = await _repository.Update(toUpdate);
                    return _mapper.Map<CarDto>(updated);

                }
                else
                {
                    var insert = _mapper.Map<Car>(car);
                    var newCar = await _repository.Add(insert);
                    return _mapper.Map<CarDto>(newCar);
                }

            }
        }
    }
}
