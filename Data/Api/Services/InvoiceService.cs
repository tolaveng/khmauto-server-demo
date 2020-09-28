using AutoMapper;
using Data.Api.Common;
using Data.Domain.Common;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repository;
        private readonly IMapper _mapper;

        public InvoiceService(IInvoiceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        public async Task Add(InvoiceDto invoice)
        {
            var insert = _mapper.Map<Invoice>(invoice);
            await _repository.Add(insert);
        }

        public async Task Archive(long id)
        {
            await _repository.Archive(id);
        }

        public async Task<PagedResponse<InvoiceDto>> GetAllPaged(PaginationQuery pagination)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(pagination);
            var totalCount = await _repository.GetCount();
            var invoices = await _repository.GetAllPaged(paginationFilter);
            var data = _mapper.Map<List<InvoiceDto>>(invoices);
            return new PagedResponse<InvoiceDto>(data, totalCount, pagination);
        }

        public async Task<PagedResponse<InvoiceDto>> GetByQuery(PaginationQuery pagination, InvoiceQuery query)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(pagination);
            var totalCount = await _repository.GetCountByQuery(query);
            var invoices = await _repository.GetByQuery(paginationFilter, query);
            var data = _mapper.Map<List<InvoiceDto>>(invoices);
            return new PagedResponse<InvoiceDto>(data, totalCount, pagination);
        }

        public async Task Update(InvoiceDto invoice)
        {
            var update = _mapper.Map<Invoice>(invoice);
            await _repository.Update(update);
        }
    }
}
