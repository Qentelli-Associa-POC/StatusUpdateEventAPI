using Associa.Service.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Associa.Service.DAL.Interfaces
{
   public interface IInvoiceUpdateEventRepository
    {
        Task<bool> UpdateInvoiceStatus(Models.Invoice invoice);

        Task<bool> UpdateInvoiceTracker(InvoiceTracker invoice);

        Task<bool> AddInvoiceTracker(List<InvoiceTracker> newInvoiceList);

        Task<bool> UpdateWorkFlowStatusList(List<WorkFlowStatus> workFlow);

        Task<bool> UpdateWorkFlowStatus(WorkFlowStatus workflow);

        Task<List<WorkFlowStatus>> GetWorkFlowStatus(Guid invoiceId, Guid personId);

        Task<Invoice> GetInvoiceDetail(Guid invoiceId);

        Task<List<InvoiceTracker>> GetInvoiceTracker(Guid invoiceId, Guid personId);


    }
}
