using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Api.Common;
using Data.Api.Services;
using Data.DTO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
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
        private readonly IServiceService _serviceService;
        private readonly IServiceIndexService _serviceIndexService;
        private readonly IQuoteService _quoteService;

        public InvoiceController(IInvoiceService invoiceService, IServiceService serviceService,
            IServiceIndexService serviceIndexService,
            IQuoteService quoteService)
        {
            _invoiceService = invoiceService;
            _serviceService = serviceService;
            _serviceIndexService = serviceIndexService;
            _quoteService = quoteService;
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


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(long id)
        {
            await _invoiceService.Archive(id);
            return Ok();
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
                if (!string.IsNullOrWhiteSpace(filter.InvoiceDate) && DateTime.TryParse(filter.InvoiceDate, out var parsedDate)) {
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


        [HttpGet("getSummaryReport")]
        public async Task<ActionResult> GetSummaryReport([FromQuery] PageRequest pageRequest, [FromQuery] SummaryReportFilter filter = null)
        {
            var pageQuery = new PaginationQuery()
            {
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
            
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


        [HttpGet("downloadSummaryReport")]
        public async Task<ActionResult> DownloadSummaryReport([FromQuery] SummaryReportFilter filter = null)
        {
            var pageQuery = new PaginationQuery()
            {
                PageNumber = 1,
                PageSize = int.MaxValue
            };
            
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

            var response = await _invoiceService.GetSummaryReport(pageQuery, fromDate, toDate, "InvoiceNo", "ASC");
            if (response != null)
            {
                var fileName = $"KHM_Invoice_{fromDate.ToString("dd-MM-yyyy")}_{toDate.ToString("dd-MM-yyyy")}.xlsx";
                using (var mem = new MemoryStream())
                {
                    var spreadsheetDocument = SpreadsheetDocument.Create(mem, SpreadsheetDocumentType.Workbook);
                    // Add a WorkbookPart to the document.
                    WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                    workbookpart.Workbook = new Workbook();

                    // Add a WorksheetPart to the WorkbookPart.
                    WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    // Add Sheets to the Workbook.
                    Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                    // Append a new worksheet and associate it with the workbook.
                    Sheet sheet = new Sheet()
                    {
                        Id = spreadsheetDocument.WorkbookPart.
                        GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "Report"
                    };
                    sheets.Append(sheet);

                    Worksheet worksheet = worksheetPart.Worksheet;
                    SheetData sheetData = worksheet.GetFirstChild<SheetData>();
                    
                    Row rowHeader = new Row();
                    rowHeader.Append(CreateCell("Invoice No"));
                    rowHeader.Append(CreateCell("Date"));
                    rowHeader.Append(CreateCell("Services"));
                    rowHeader.Append(CreateCell("SubTotal"));
                    rowHeader.Append(CreateCell("Discount"));
                    rowHeader.Append(CreateCell("Total(ex GST)"));
                    rowHeader.Append(CreateCell("GST"));
                    rowHeader.Append(CreateCell("Total(in GST)"));
                    sheetData.Append(rowHeader);


                    //Data
                    foreach(var data in response.Data)
                    {
                        Row row = new Row();
                        row.Append(CreateCell(data.InvoiceNo.ToString()));
                        row.Append(CreateCell(data.InvoiceDate));
                        row.Append(CreateCell(data.Services));
                        row.Append(CreateCell(data.SubTotal.ToString("0.##"), "number"));
                        row.Append(CreateCell(data.Discount.ToString("0.##"), "number"));
                        row.Append(CreateCell(data.AmountTotal.ToString("0.##"), "number"));
                        row.Append(CreateCell(data.GstTotal.ToString("0.##"), "number"));
                        row.Append(CreateCell((data.AmountTotal + data.GstTotal).ToString("0.##"), "number"));
                        sheetData.Append(row);
                    }


                    // save close
                    workbookpart.Workbook.Save();
                    spreadsheetDocument.Close();

                    //return File(
                    //    fileContents: mem.ToArray(),
                    //    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    //    fileDownloadName: fileName
                    //    );
                    return File(mem.ToArray(), "application/octet-stream", fileName);
                }

            }
            return Ok();
        }


        private Cell CreateCell(string text, string type = "string")
        {
            Cell cell = new Cell();
            cell.CellValue = new CellValue(text);
            if (type == "date")
            {
                cell.DataType = CellValues.Date;
            }
            else if (type == "number")
            {
                cell.DataType = CellValues.Number;
            }
            else
            {
                cell.DataType = CellValues.String;
            }
            return cell;
        }

        //private string CalTotal(decimal price, int qty)
        //{
        //    return (price * qty).ToString("0.##");
        //}

        //private string CalGst(decimal price, int qty, float gst)
        //{
        //    return ((price * qty) / (decimal)(gst + 1)).ToString("0.##");
        //}

        //private string CalTotalExGst(decimal price, int qty, float gst)
        //{
        //    var calGst = (price * qty) / (decimal)(gst + 1);
        //    return ((price * qty) - calGst).ToString("0.##");
        //}


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

        
        [HttpGet("fromQuote/{quoteId}")]
        public async Task<ActionResult> Fromquote(string quoteId)
        {
            if (string.IsNullOrWhiteSpace(quoteId) || !int.TryParse(quoteId, out var newQuoteId))
            {
                return BadRequest();
            }
            var quote = await _quoteService.GetById(newQuoteId);
            if (quote == null) return BadRequest("Quote is not found");

            var services = quote.Services;
            foreach(var service in services)
            {
                service.ServiceId = 0;
            }

            var invoice = new InvoiceDto()
            {
                FullName = quote.FullName,
                Phone = quote.Phone,
                Email = quote.Email,
                Company = quote.Company,
                Abn = quote.Abn,
                Address = quote.Address,
                Car = new CarDto
                {
                    CarNo = quote.Car.CarNo,
                    ODO = quote.Car.ODO,
                    CarYear = quote.Car.CarYear,
                    CarMake = quote.Car.CarMake,
                    CarModel = quote.Car.CarModel
                },
                Note = quote.Note,
                PaymentMethod = Data.Enums.PaymentMethod.Card,
                Services = services
            };

            return Json(invoice);
        }


        [HttpGet("getserviceindex")]
        public async Task<ActionResult> GetServiceIndex(string serviceName)
        {
            var response = new List<ServiceIndexDto>();
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                var result = await _serviceIndexService.GetAll(200);
                if (result != null)
                    response = result.ToList();
            } else
            {
                var result = await _serviceIndexService.FindByServiceName(serviceName, 200);
                if (result != null)
                    response = result.ToList();
            }

            return Json(response);
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
