using AutoMapper;
using Data.Api.Common;
using Data.Domain.Repositories;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public class ServiceIndexService : IServiceIndexService
    {
        private readonly IServiceIndexRepository _repository;
        private readonly IMapper _mapper;

        public ServiceIndexService(IServiceIndexRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        public async Task DeleteById(int id)
        {
            await _repository.DeleteService(id); 
        }

        public async Task<IEnumerable<ServiceIndexDto>> FindByServiceName(string serviceName, int limit)
        {
            var services = await _repository.FindByServiceName(serviceName, limit);
            return _mapper.Map<IEnumerable<ServiceIndexDto>>(services);
        }

        public async Task<PaginationResponse<ServiceIndexDto>> FindByServiceNamePaged(string serviceName, PaginationQuery pagination)
        {
            var results = await _repository.FindByServiceNamePaged(serviceName, pagination);
            var totalCount = results.Count();
            var data = _mapper.Map<List<ServiceIndexDto>>(results);
            var hasNext = (pagination.PageNumber * pagination.PageSize) < totalCount;
            return new PaginationResponse<ServiceIndexDto>(data, totalCount, hasNext, pagination);
        }

        public async Task<IEnumerable<ServiceIndexDto>> GetAll(int limit)
        {
            var services = await _repository.GetAll(limit);
            return _mapper.Map<IEnumerable<ServiceIndexDto>>(services);
        }

        public async Task Update(int id, string serviceName)
        {
            await _repository.UpdateService(id, serviceName);
        }
    }
}
