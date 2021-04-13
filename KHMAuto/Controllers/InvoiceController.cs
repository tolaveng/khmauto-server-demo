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
        public async Task<ActionResult> GetAll([FromQuery]PageRequest pageRequest, [FromQuery] InvoiceFilter filter = null)
        {
            var pageQuery = new PaginationQuery()
            {
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
            var query = new InvoiceQuery();
            if (filter != null)
            {
                query.InvoiceNo = long.TryParse(filter.InvoiceNo, out var pInvoiceNo) ? pInvoiceNo :  0;
                query.CarNo = filter.CarNo ?? "";
                query.Customer = filter.Customer ?? "";
                if (DateTime.TryParse(filter.InvoiceDate, out var parsedDate)) {
                    if (parsedDate.Kind == DateTimeKind.Utc)
                    {
                        parsedDate = parsedDate.ToLocalTime().Date;
                    }
                    query.InvoiceDate = parsedDate;
                };
                query.SortBy = filter.SortBy;
                query.SortDir = filter.SortDir;
            }
            var response = await _invoiceService.GetByQuery(pageQuery, query);
            if (response != null)
            {
                return Json(response);
            }
            return Ok();
        }


        [HttpGet("getall")]
        public async Task<ActionResult> GetSummaryReport([FromQuery] PageRequest pageRequest, [FromQuery] SummaryReportFilter filter = null)
        {
            var pageQuery = new PaginationQuery()
            {
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
            var query = new InvoiceQuery();
            if (filter == null)
            {
                return NoContent();
            }

            if (DateTime.TryParse(filter.FromDate, out var fromDate))
            {
                if (fromDate.Kind == DateTimeKind.Utc)
                {
                    fromDate = fromDate.ToLocalTime().Date;
                }
            }
            else
            {
                return BadRequest("Invalid From Date");
            }

            if (DateTime.TryParse(filter.ToDate, out var toDate))
            {
                if (toDate.Kind == DateTimeKind.Utc)
                {
                    toDate = toDate.ToLocalTime().Date;
                }
            }
            else
            {
                return BadRequest("Invalid To Date");
            }

            var response = await _invoiceService.GetSummaryReport(pageQuery, fromDate, toDate, filter.SortBy, filter.SortDir);
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
