using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Api.Services;
using Data.DTO;
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


        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDto>> Get(long id)
        {
            var invoice = await _invoiceService.GetById(id);
            if (invoice != null)
            {
                return Json(invoice);
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
    }
}
