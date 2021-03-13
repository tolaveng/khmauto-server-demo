﻿using System;
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


        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDto>> GetById(long id)
        {
            var invoice = await _invoiceService.GetById(id);
            if (invoice != null)
            {
                return Json(invoice);
            }
            return NoContent();
        }


        [HttpGet("no/{no}")]
        public async Task<ActionResult<InvoiceDto>> GetByNo(long no)
        {
            var invoice = await _invoiceService.GetByNo(no);
            if (invoice != null)
            {
                return Json(invoice);
            }
            return NoContent();
        }


        [HttpGet("getall")]
        public async Task<ActionResult<InvoiceDto>> GetAll([FromQuery]PageRequest pageRequest)
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
