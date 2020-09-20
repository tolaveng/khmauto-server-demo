using AutoMapper;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _repository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public ServiceService(IServiceRepository repository, IInvoiceRepository invoiceRepository, IMapper mapper)
        {
            _repository = repository;
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;

        }
        public async Task Add(ServiceDto service)
        {
            var insert = _mapper.Map<Service>(service);
            await _repository.Add(insert);
        }

        public async Task<bool> Delete(long id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<ServiceDto>> GetByInvoiceId(long invoiceId)
        {
            var services = await _repository.GetByInvoiceId(invoiceId);
            return _mapper.Map<List<ServiceDto>>(services);
        }

        public async Task<ServiceDto> GetById(long id)
        {
            var service = await _repository.GetById(id);
            return _mapper.Map<ServiceDto>(service);
        }

        public async Task<ServiceDto> GetByName(string name)
        {
            var service = await _repository.GetByName(name);
            return _mapper.Map<ServiceDto>(service);
        }

        public async Task Update(ServiceDto service)
        {
            var entry = await _repository.GetById(service.Id);
            if (entry == null)
            {
                throw new ArgumentException("Service is not found");
            }
            var update = _mapper.Map<Service>(service);
            await _repository.Update(update);
        }

        public async Task<IEnumerable<ServiceDto>> GetByCarId(long carId)
        {
            var invoices = await _invoiceRepository.GetByCarId(carId);
            var result = new List<ServiceDto>();
            foreach(var invoice in invoices)
            {
                var services = await _repository.GetByInvoiceId(invoice.Id);
                result.AddRange(_mapper.Map<List<ServiceDto>>(services));
            }
            return result;
        }
    }
}
