using Associa.Service.DAL.Interfaces;
using Associa.Service.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Associa.Service.DAL.Repositories
{
    public class InvoiceUpdateEventRepository : IInvoiceUpdateEventRepository
    {

        public async Task<bool> AddInvoiceTracker(List<InvoiceTracker> newInvoiceList)
        {
            using (var context = new AssociaSqlContext())
            {
                context.InvoiceTracker.AddRange(newInvoiceList);
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> UpdateInvoiceStatus(Models.Invoice invoice)
        {
            using (var context = new AssociaSqlContext())
            {
                context.Invoice.Update(invoice);
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> UpdateInvoiceTracker(InvoiceTracker invoice)
        {
            using (var context = new AssociaSqlContext())
            {
                context.InvoiceTracker.Update(invoice);
                return await context.SaveChangesAsync() > 0;
            }
        }
        public async Task<bool> UpdateWorkFlowStatusList(List<WorkFlowStatus> workFlow)
        {
            using (var context = new AssociaSqlContext())
            {
                context.WorkFlowStatus.UpdateRange(workFlow);
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> UpdateWorkFlowStatus(WorkFlowStatus workFlow)
        {
            using (var context = new AssociaSqlContext())
            {
                context.WorkFlowStatus.Update(workFlow);
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<List<WorkFlowStatus>> GetWorkFlowStatus(Guid invoiceId)
        {
            using (var context = new AssociaSqlContext())
            {
                return await context.WorkFlowStatus
                    .Include(wf => wf.TemplateStep)
                    .Where(wf => wf.InvoiceId == invoiceId)
                    .OrderBy(wf => wf.SequenceId).ToListAsync();
            }
        }

        public async Task<Invoice> GetInvoiceDetail(Guid invoiceId)
        {
            using (var context = new AssociaSqlContext())
            {
                return await context.Invoice.Where(i => i.InvoiceId == invoiceId)
                    .Include(i => i.WorkFlowStatus)
                        .ThenInclude(wf => wf.TemplateStep).FirstOrDefaultAsync();
            }
        }

        public async Task<List<InvoiceTracker>> GetInvoiceTracker(Guid invoiceId)
        {
            using (var context = new AssociaSqlContext())
            {
                return await context.InvoiceTracker.Where(i => i.InvoiceId == invoiceId).ToListAsync();
            }
        }

        public async Task<Person> GetUserDetails(Guid userId)
        {
            try
            {
                using (var context = new AssociaSqlContext())
                {
                    return await context.Person.Include(x => x.Role).FirstOrDefaultAsync(x => x.PersonId == userId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
