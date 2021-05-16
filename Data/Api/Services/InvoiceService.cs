using AutoMapper;
using Data.Api.Common;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.DTO;
using Data.Enums;
using Data.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ICarService _carService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IServiceIndexRepository _serviceIndexRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository,
            IServiceRepository serviceRepository,
            ICarService carService,
            ITransactionRepository transactionRepository,
            IServiceIndexRepository serviceIndexRepository,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _serviceRepository = serviceRepository;
            _carService = carService;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _serviceIndexRepository = serviceIndexRepository;
        }

        public async Task<InvoiceDto> Create(InvoiceDto invoice)
        {
            if (invoice.UserId == 0)
            {
                throw new ArgumentException("User is required");
            }

            var newInvoice = _mapper.Map<Invoice>(invoice);
            if (newInvoice.InvoiceDate == null)
            {
                newInvoice.InvoiceDate = DateTime.Now.Date;
            }
            else
            {
                newInvoice.InvoiceDate = newInvoice.InvoiceDate.Date;
            }
            
            newInvoice.ModifiedDateTime = DateTime.Now;
            var isPaid = newInvoice.PaymentMethod != PaymentMethod.Unpaid;
            if (isPaid)
            {
                newInvoice.PaidDate = DateTime.Now;
            }

            // make auto increment, new service is negative (-)
            foreach(var service in newInvoice.Services)
            {
                if (service.ServiceId < 1) service.ServiceId = 0;
                service.Quote = null;
                service.QuoteId = null;
            }

            using (var transaction = await _transactionRepository.BeginTransaction())
            {
                try
                {
                    var maxInvoiceNo = await _invoiceRepository.GetMaxInvoiceNo();
                    newInvoice.InvoiceNo = maxInvoiceNo + 1;

                    var car = await _carService.CreateOrUpdate(invoice.Car);
                    newInvoice.CarNo = car.CarNo;
                    newInvoice.Car = null;
                    
                    newInvoice = await _invoiceRepository.Add(newInvoice);

                    // update service index for auto complete
                    foreach (var ser in newInvoice.Services)
                    {
                        await _serviceIndexRepository.AddOrUpdateService(ser.ServiceName, ser.ServicePrice);
                    }

                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }          
                await transaction.CommitAsync();
                return _mapper.Map<InvoiceDto>(newInvoice);
            }
        }


        public async Task Archive(long id)
        {
            var invoice = await _invoiceRepository.GetById(id);
            if (invoice == null)
            {
                throw new ArgumentException("Invoice is not found");
            }
            await _invoiceRepository.Archive(id);
        }

        public async Task<PaginationResponse<InvoiceDto>> GetAllPaged(PaginationQuery pagination)
        {
            var totalCount = await _invoiceRepository.GetCount();
            var invoices = await _invoiceRepository.GetAllPaged(pagination);
            var data = _mapper.Map<List<InvoiceDto>>(invoices);
            var hasNext = (pagination.PageNumber * pagination.PageSize) < totalCount;
            return new PaginationResponse<InvoiceDto>(data, totalCount, hasNext, pagination);
        }


        public async Task<InvoiceDto> GetById(long id)
        {
            var invoice = await _invoiceRepository.GetById(id);
            if (invoice == null) return null;
            return _mapper.Map<InvoiceDto>(invoice);
        }


        public async Task<InvoiceDto> GetByNo(long no)
        {
            var invoice = await _invoiceRepository.GetByNo(no);
            if (invoice == null) return null;
            return _mapper.Map<InvoiceDto>(invoice);
        }


        public async Task<PaginationResponse<InvoiceDto>> GetByQuery(PaginationQuery pagination, InvoiceQuery query)
        {
            var totalCount = await _invoiceRepository.GetCountByQuery(query);
            var invoices = await _invoiceRepository.GetByQuery(pagination, query);
            var data = _mapper.Map<List<InvoiceDto>>(invoices);
            var hasNext = (pagination.PageNumber * pagination.PageSize) < totalCount;
            return new PaginationResponse<InvoiceDto>(data, totalCount, hasNext, pagination);
        }


        public async Task Update(InvoiceDto invoice)
        {
            if (invoice.UserId == 0)
            {
                throw new ArgumentException("User is required");
            }

            var updateInvoice = _mapper.Map<Invoice>(invoice);
            updateInvoice.InvoiceDate = updateInvoice.InvoiceDate.Date;
            updateInvoice.ModifiedDateTime = DateTime.Now;

            // make auto increment
            foreach (var service in updateInvoice.Services)
            {
                if (service.ServiceId < 1) service.ServiceId = 0;
                service.Invoice = null;
                service.InvoiceId = null;
            }

            using (var transaction = await _transactionRepository.BeginTransaction())
            {
                try
                {
                    var existInvoice = await _invoiceRepository.GetById(updateInvoice.InvoiceId);
                    if (existInvoice == null)
                    {
                        throw new ArgumentException("Invoice Id is not found.");
                    }
                    
                    // No track Cars
                    var car = await _carService.CreateOrUpdate(invoice.Car);
                    updateInvoice.CarNo = car.CarNo;
                    updateInvoice.Car = null;

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
