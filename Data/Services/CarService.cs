using AutoMapper;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<List<CarDto>> GetAll()
        {
            var cars = await _repository.GetAll();
            return _mapper.Map<List<CarDto>>(cars);
        }

        public async Task<CarDto> GetById(long id)
        {
            var car = await _repository.GetById(id);
            return _mapper.Map<CarDto>(car);
        }

        public async Task<CarDto> GetByCarNo(string carNo)
        {
            var car = await _repository.GetByCarNo(carNo);
            return _mapper.Map<CarDto>(car);
        }
    }
}
