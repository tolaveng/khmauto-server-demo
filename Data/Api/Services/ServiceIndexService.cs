using AutoMapper;
using Data.Domain.Repositories;
using Data.DTO;
using System;
using System.Collections.Generic;
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
        public async Task<IEnumerable<ServiceIndexDto>> FindByServiceName(string serviceName)
        {
            var services = await _repository.FindByServiceName(serviceName);
            return _mapper.Map<IEnumerable<ServiceIndexDto>>(services);
        }
    }
}
