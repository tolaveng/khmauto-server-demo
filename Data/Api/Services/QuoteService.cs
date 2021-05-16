using AutoMapper;
using Data.Api.Common;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.DTO;
using Data.Services;
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
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICarService _carService;

        public QuoteService(IQuoteRepository repository, IMapper mapper, ITransactionRepository transactionRepository,
            ICarService carService)
        {
            _repository = repository;
            _mapper = mapper;
            _transactionRepository = transactionRepository;
            _carService = carService;
        }

        public async Task<QuoteDto> Create(QuoteDto quote)
        {
            if (quote.UserId == 0)
            {
                throw new ArgumentException("User is required");
            }
            var newQuote = _mapper.Map<Quote>(quote);

            if (newQuote.QuoteDate == null)
            {
                newQuote.QuoteDate = DateTime.Now.Date;
            }
            else
            {
                newQuote.QuoteDate = newQuote.QuoteDate.Date;
            }
            newQuote.ModifiedDateTime = DateTime.Now;

            // make auto increment, new service is negative (-)
            foreach (var service in newQuote.Services)
            {
                if (service.ServiceId < 1) service.ServiceId = 0;
                service.Invoice = null;
                service.InvoiceId = null;
            }

            using (var transaction = await _transactionRepository.BeginTransaction())
            {
                try
                {
                    var car = await _carService.CreateOrUpdate(quote.Car);
                    newQuote.CarNo = car.CarNo;
                    newQuote.Car = null;

                    newQuote = await _repository.Add(newQuote);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
                await transaction.CommitAsync();
                return _mapper.Map<QuoteDto>(newQuote);
            }
        }

        public async Task Delete(long id)
        {
            await _repository.Delete(id);
        }

        public async Task<PaginationResponse<QuoteDto>> GetAllPaged(PaginationQuery pagination)
        {
            var totalCount = await _repository.GetCount();
            var quotes = await _repository.GetAllPaged(pagination);
            var data = _mapper.Map<List<QuoteDto>>(quotes);
            var hasNext = (pagination.PageNumber * pagination.PageSize) < totalCount;
            return new PaginationResponse<QuoteDto>(data, totalCount, hasNext, pagination);
        }

        public async Task<QuoteDto> GetById(long id)
        {
            var quote = await _repository.GetById(id);
            return _mapper.Map<QuoteDto>(quote);
        }

        public async Task<PaginationResponse<QuoteDto>> GetByQuery(PaginationQuery pagination, QuoteQuery query)
        {
            var totalCount = await _repository.GetCountByQuery(query);
            var quotes = await _repository.GetByQuery(pagination, query);
            var data = _mapper.Map<List<QuoteDto>>(quotes);
            var hasNext = (pagination.PageNumber * pagination.PageSize) < totalCount;
            return new PaginationResponse<QuoteDto>(data, totalCount, hasNext, pagination);
        }

        public async Task Update(QuoteDto quote)
        {
            if (quote.UserId == 0)
            {
                throw new ArgumentException("User is required");
            }

            var updateQuote = _mapper.Map<Quote>(quote);
            updateQuote.QuoteDate = updateQuote.QuoteDate.Date;
            updateQuote.ModifiedDateTime = DateTime.Now;

            // make auto increment
            foreach (var service in updateQuote.Services)
            {
                if (service.ServiceId < 1) service.ServiceId = 0;
                service.Invoice = null;
                service.InvoiceId = null;
            }

            using (var transaction = await _transactionRepository.BeginTransaction())
            {
                try
                {
                    var existQuote = await _repository.GetById(updateQuote.QuoteId);
                    if (existQuote == null)
                    {
                        throw new ArgumentException("Quote Id is not found.");
                    }

                    // No track Cars
                    var car = await _carService.CreateOrUpdate(quote.Car);
                    updateQuote.CarNo = car.CarNo;
                    updateQuote.Car = null;

                    await _repository.Update(updateQuote);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
                await transaction.CommitAsync();
            }
        }
    }
}
