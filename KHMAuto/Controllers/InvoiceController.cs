using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IInvoiceService _invoiceService;
        private readonly IServiceIndexService _serviceIndexService;

        public InvoiceController(IInvoiceService invoiceService, IServiceIndexService serviceIndexService)
        {
            _invoiceService = invoiceService;
            _serviceIndexService = serviceIndexService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var invoice = await _invoiceService.GetById(id);
            if (invoice != null)
            {
                return Json(invoice);
            }
            return NoContent();
        }


        [HttpGet("no/{no}")]
        public async Task<ActionResult> GetByNo(long no)
        {
            var invoice = await _invoiceService.GetByNo(no);
            if (invoice != null)
            {
                return Json(invoice);
            }
            return NoContent();
        }


        [HttpGet("getall")]
        public async Task<ActionResult> GetAll([FromQuery]PageRequest pageRequest)
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
            try
            {
                var newInvoice = await _invoiceService.Create(invoice);
                var response = new ResponseResult<InvoiceDto>();
                response.success = true;
                response.message = "";
                response.data = newInvoice;
                return Json(response);
            }
            catch(Exception e)
            {
                var response = new ResponseResult<string>();
                response.success = false;
                response.message = "Unable to create new invoice";
                response.debugMessage = $"{e.Message} {e.StackTrace}";
                Console.WriteLine(e.ToString());
                return Json(response);
            }
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
                response.debugMessage = $"{e.Message} {e.StackTrace}";

                return Json(response);
            }
            response.success = true;
            response.message = "";
            return Json(response);
        }


        [HttpGet("getserviceindex")]
        public async Task<ActionResult> GetServiceIndex()
        {
            var response = await _serviceIndexService.GetAll(100);
            if (response != null)
            {
                return Json(response);
            }
            return Ok();
        }


        //[HttpGet("print/{id}")]
        //public async Task<ActionResult> PrintById(long id)
        //{
        //    var invoice = await _invoiceService.GetById(id);
        //    if (invoice == null)
        //    {
        //        return NoContent();
        //    }
        //    var htmlPath = Path.Combine(Directory.GetCurrentDirectory(), "HtmlTemplates", "InvoicePrint.html");
        //    var html = System.IO.File.ReadAllText(htmlPath);

        //    var globalSettings = new GlobalSettings
        //    {
        //        ColorMode = ColorMode.Color,
        //        Orientation = Orientation.Portrait,
        //        PaperSize = PaperKind.A4,
        //        Margins = new MarginSettings { Top = 11 },
        //        DocumentTitle = "Invoice Report",
        //        //Out = @"C:\temp\Invoice_Report.pdf"
        //    };

        //    var objectSettings = new ObjectSettings
        //    {
        //        PagesCount = true,
        //        HtmlContent = html,
        //        //WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "InvoiceStyles.css") },
        //        WebSettings = { DefaultEncoding = "utf-8" },
        //        //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
        //        //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
        //    };

        //    var pdf = new HtmlToPdfDocument()
        //    {
        //        GlobalSettings = globalSettings,
        //        Objects = { objectSettings }
        //    };

        //    var file = _converter.Convert(pdf);
        //    return File(file, "application/pdf");
        //}

    }
}
