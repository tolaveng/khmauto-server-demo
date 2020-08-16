using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface ICarService
    {
        Task<List<CarDto>> GetAll();
        Task<CarDto> GetById(long id);
        Task<CarDto> GetByCarNo(string carNo);
        Task Add(CarDto car);
        Task Delete(long id);

    }
}
