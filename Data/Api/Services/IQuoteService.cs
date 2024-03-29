﻿using Data.Api.Common;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public interface IQuoteService
    {
        Task<PaginationResponse<QuoteDto>> GetAllPaged(PaginationQuery pagination);
        Task<PaginationResponse<QuoteDto>> GetByQuery(PaginationQuery pagination, QuoteQuery query);
        Task<QuoteDto> GetById(long id);
        Task<QuoteDto> Create(QuoteDto quote);
        Task Delete(long id);
        Task Update(QuoteDto quote);
    }
}
