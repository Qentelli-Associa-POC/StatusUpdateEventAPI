using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class InvoiceWorkFlow
    {
        public Guid InvoiceWorkFlowId { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid WorkFlowMasterId { get; set; }
        public bool IsCompleted { get; set; }
        public decimal Sequence { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual WorkFlowMaster WorkFlowMaster { get; set; }
    }
}
