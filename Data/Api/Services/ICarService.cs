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
        Task<PagedResponse<CarDto>> GetAllPaged(PaginationQuery pagination);
        Task<PagedResponse<CarDto>> FindByCarNoPaged(string carNo, PaginationQuery pagination);
        Task<CarDto> GetById(long id);
        Task<CarDto> GetByCarNo(string carNo);
        Task Add(CarDto car);
        Task Delete(long id);
    }
}
