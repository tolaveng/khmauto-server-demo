using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Api.Common;
using Data.Api.Services;
using Data.DTO;
using KHMAuto.Requests;
using KHMAuto.Responses;
using Microsoft.AspNetCore.Mvc;

namespace KHMAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : Controller
    {
        private readonly IQuoteService _quoteService;
        private readonly IServiceService _serviceService;
        private readonly IServiceIndexService _serviceIndexService;

        public QuoteController(IQuoteService quoteService, IServiceService serviceService, IServiceIndexService serviceIndexService)
        {
            _quoteService = quoteService;
            _serviceService = serviceService;
            _serviceIndexService = serviceIndexService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var quote = await _quoteService.GetById(id);
            if (quote != null)
            {
                return Json(quote);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(long id)
        {
            await _quoteService.Delete(id);
            return Ok();
        }


        [HttpGet("getall")]
        public async Task<ActionResult> GetAll([FromQuery] PageRequest pageRequest, [FromQuery] QuoteFilter filter = null)
        {
            var pageQuery = new PaginationQuery()
            {
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
            var query = new QuoteQuery();
            if (filter != null)
            {
                query.QuoteId = long.TryParse(filter.QuoteId, out var pId) ? pId : 0;
                query.CarNo = filter.CarNo ?? "";
                query.Customer = filter.Customer ?? "";
                if (!string.IsNullOrWhiteSpace(filter.QuoteDate) && DateTime.TryParse(filter.QuoteDate, out var parsedDate))
                {
                    if (parsedDate.Kind == DateTimeKind.Utc)
                    {
                        parsedDate = parsedDate.ToLocalTime().Date;
                    }
                    query.QuoteDate = parsedDate;
                };
                query.SortBy = filter.SortBy;
                query.SortDir = filter.SortDir;
            }
            var response = await _quoteService.GetByQuery(pageQuery, query);
            if (response != null)
            {
                return Json(response);
            }
            return Ok();
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] QuoteDto quote)
        {
            try
            {
                var newQuote = await _quoteService.Create(quote);
                var response = new ResponseResult<QuoteDto>();
                response.success = true;
                response.message = "";
                response.data = newQuote;
                return Json(response);
            }
            catch (Exception e)
            {
                var response = new ResponseResult<string>();
                response.success = false;
                response.message = "Unable to create new quote";
                response.debugMessage = $"{e.Message} {e.StackTrace}";
                Console.WriteLine(e.ToString());
                return Json(response);
            }
        }


        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] QuoteDto quote)
        {
            var response = new ResponseResult<string>();
            try
            {
                await _quoteService.Update(quote);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = "Unable to update quote";
                response.debugMessage = $"{e.Message} {e.StackTrace}";

                return Json(response);
            }
            response.success = true;
            response.message = "";
            return Json(response);
        }

    }
}
