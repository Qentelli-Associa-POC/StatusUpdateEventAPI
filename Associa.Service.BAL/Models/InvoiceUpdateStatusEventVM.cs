using System;
using System.Collections.Generic;
using System.Text;

namespace Associa.Service.BAL.Models
{
    public class InvoiceUpdateStatusEventVM
    {
        public string KakfaTopic { get; set; }
        public Guid UserId { get; set; }
        public List<InvoiceStatusVM> invoiceStatusVM { get; set; }
    }
}
