﻿using Data.Api.Common;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> GetById(long id);
        Task<InvoiceDto> GetByNo(long no);
        Task<PaginationResponse<InvoiceDto>> GetAllPaged(PaginationQuery pagination);
        Task<PaginationResponse<InvoiceDto>> GetByQuery(PaginationQuery pagination, InvoiceQuery query);
        Task<InvoiceDto> Create(InvoiceDto invoice);
        Task Archive(long id);
        Task Update(InvoiceDto invoice);

        Task<PaginationResponse<SummaryReport>> GetSummaryReport(PaginationQuery pagination, DateTime fromDate, DateTime toDate, string sortBy = null, string sortDir = null);
    }
}
