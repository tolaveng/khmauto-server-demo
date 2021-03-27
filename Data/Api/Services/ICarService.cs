using Data.Api.Common;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface ICarService
    {
        Task<PaginationResponse<CarDto>> GetAllPaged(PaginationQuery pagination);
        Task<PaginationResponse<CarDto>> FindByCarNoPaged(string carNo, PaginationQuery pagination);
        Task<CarDto> GetByCarNo(string carNo);
        Task Add(CarDto car);
        Task Delete(string carNo);
        Task Update(CarDto car);
        Task<CarDto> CreateOrUpdate(CarDto car);
    }
}
