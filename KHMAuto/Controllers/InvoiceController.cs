using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Api.Common;
using Data.Api.Services;
using Data.DTO;
using KHMAuto.Requests;
using KHMAuto.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KHMAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : Controller
    {
        private IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }


        [HttpGet("getbyid")]
        public async Task<ActionResult<InvoiceDto>> GetById([FromBody] IdRequest idRequest)
        {
            var invoice = await _invoiceService.GetById(idRequest.id);
            if (invoice != null)
            {
                return Json(invoice);
            }
            return Ok();
        }


        [HttpGet("getbyno")]
        public async Task<ActionResult<InvoiceDto>> GetByNo([FromBody] NoRequest noRequest)
        {
            var invoice = await _invoiceService.GetByNo(noRequest.No);
            if (invoice != null)
            {
                return Json(invoice);
            }
            return Ok();
        }


        [HttpGet("getall")]
        public async Task<ActionResult<InvoiceDto>> GetAll([FromBody] PageRequest pageRequest)
        {
            var pageQuery = new PaginationQuery()
            {
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
            var response = await _invoiceService.GetAllPaged(pageQuery);
            if (response != null)
            {
                return Json(response);
            }
            return Ok();
        }


        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] InvoiceDto invoice)
        {
            var response = new ResponseResult<string>();
            try
            {
                await _invoiceService.Create(invoice);
            }
            catch(Exception e)
            {
                response.success = false;
                response.message = "Unable to create new invoice";
                Console.WriteLine(e.ToString());
                return Json(response);
            }
            response.success = true;
            response.message = "";
            return Json(response);
        }


        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] InvoiceDto invoice)
        {
            var response = new ResponseResult<string>();
            try
            {
                await _invoiceService.Update(invoice);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = "Unable to update invoice";
                Console.WriteLine(e.ToString());
                return Json(response);
            }
            response.success = true;
            response.message = "";
            return Json(response);
        }
    }
}
