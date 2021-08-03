using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class SummaryReport
    {
        public long InvoiceNo {get; set; }
        public string InvoiceDate { get; set; }
        public string Services { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal AmountTotal { get; set; }
        public decimal GstTotal { get; set; }

        internal static SummaryReport FromInvoice(Invoice invoice)
        {
            var serviceNames = string.Join("\n", invoice.Services.Select(ser => ser.ServiceName));
            var subTotal = invoice.Services.Sum(ser => ser.ServicePrice * ser.ServiceQty);
            var totalAmount = subTotal - invoice.Discount;  // exclude gst
            var invoiceGst = invoice.Gst > 0 ? (decimal)invoice.Gst : 10m;
            var gstTotal = Math.Round(totalAmount * invoiceGst / 100, 2);

            return new SummaryReport()
            {
                InvoiceDate = invoice.InvoiceDate.ToString("dd/MM/yyyy"),
                InvoiceNo = invoice.InvoiceNo,
                Services = serviceNames,
                SubTotal = subTotal,
                Discount = invoice.Discount,
                AmountTotal = totalAmount,
                GstTotal = gstTotal
            };
        }
    }
}
