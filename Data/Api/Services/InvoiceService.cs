using AutoMapper;
using Data.Api.Common;
using Data.Domain.Common;
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
    public class InvoiceService : IInvoiceService
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ICustomerService _customerService;
        private readonly ICarService _carService;
        private readonly ITransactionRepository _transactionRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository,
            IServiceRepository serviceRepository,
            ICustomerService customerService,
            ICarService carService,
            ITransactionRepository transactionRepository,
        IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _serviceRepository = serviceRepository;
            _customerService = customerService;
            _carService = carService;
            _transactionRepository = transactionRepository;
            _mapper = mapper;

        }

        public async Task Create(InvoiceDto invoice)
        {
            if (invoice.UserId == 0)
            {
                throw new ArgumentException("User is required");
            }

            var newInvoice = _mapper.Map<Invoice>(invoice);
            using (var transaction = await _transactionRepository.BeginTransaction())
            {
                try
                {
                    var customer = await _customerService.CreateOrUpdate(invoice.Customer);
                    newInvoice.CustomerId = customer.CustomerId;
                    newInvoice.Customer = null;
                    
                    var car = await _carService.CreateOrUpdate(invoice.Car);
                    newInvoice.CarId = car.CarId;
                    newInvoice.Car = null;
                    
                    newInvoice = await _invoiceRepository.Add(newInvoice);
                }catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
                await transaction.CommitAsync();
            }
                
        }


        public async Task Archive(long id)
        {
            await _invoiceRepository.Archive(id);
        }

        public async Task<PagedResponse<InvoiceDto>> GetAllPaged(PaginationQuery pagination)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(pagination);
            var totalCount = await _invoiceRepository.GetCount();
            var invoices = await _invoiceRepository.GetAllPaged(paginationFilter);
            var data = _mapper.Map<List<InvoiceDto>>(invoices);
            return new PagedResponse<InvoiceDto>(data, totalCount, pagination);
        }

        public async Task<InvoiceDto> GetById(long id)
        {
            var invoice = await _invoiceRepository.GetById(id);
            if (invoice == null) return null;
            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task<PagedResponse<InvoiceDto>> GetByQuery(PaginationQuery pagination, InvoiceQuery query)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(pagination);
            var totalCount = await _invoiceRepository.GetCountByQuery(query);
            var invoices = await _invoiceRepository.GetByQuery(paginationFilter, query);
            var data = _mapper.Map<List<InvoiceDto>>(invoices);
            return new PagedResponse<InvoiceDto>(data, totalCount, pagination);
        }

        public async Task Update(InvoiceDto invoice)
        {
            if (invoice.UserId == 0)
            {
                throw new ArgumentException("User is required");
            }

            var updateInvoice = _mapper.Map<Invoice>(invoice);
            using (var transaction = await _transactionRepository.BeginTransaction())
            {
                try
                {
                    await _invoiceRepository.Update(updateInvoice);
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
