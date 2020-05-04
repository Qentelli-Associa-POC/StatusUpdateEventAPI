using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VM = Associa.Service.BAL.Models;

namespace Associa.Service.BAL.Interfaces
{
   public interface IInvoiceUpdateEventLogic
    {
        Task<bool> UpdateInvoiceStatus(Guid personId, List<VM.InvoiceStatusVM> invoiceStatusList);
    }
}
