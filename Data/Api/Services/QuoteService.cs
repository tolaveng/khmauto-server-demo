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
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _repository;
        private readonly IMapper _mapper;

        public QuoteService(IQuoteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        public async Task Add(QuoteDto quote)
        {
            var insert = _mapper.Map<Quote>(quote);
            await _repository.Add(insert);
        }

        public async Task Delete(long id)
        {
            await _repository.Delete(id);
        }

        public async Task<PagedResponse<QuoteDto>> GetAllPaged(PaginationQuery pagination)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(pagination);
            var totalCount = await _repository.GetCount();
            var quotes = await _repository.GetAllPaged(paginationFilter);
            var data = _mapper.Map<List<QuoteDto>>(quotes);
            return new PagedResponse<QuoteDto>(data, totalCount, pagination);
        }

        public async Task<PagedResponse<QuoteDto>> GetByQuery(PaginationQuery pagination, InvoiceQuery query)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(pagination);
            var totalCount = await _repository.GetCountByQuery(query);
            var quotes = await _repository.GetByQuery(paginationFilter, query);
            var data = _mapper.Map<List<QuoteDto>>(quotes);
            return new PagedResponse<QuoteDto>(data, totalCount, pagination);
        }

        public async Task Update(QuoteDto quote)
        {
            var update = _mapper.Map<Quote>(quote);
            await _repository.Update(update);
        }
    }
}
