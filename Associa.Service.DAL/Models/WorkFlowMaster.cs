using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class WorkFlowMaster
    {
        public WorkFlowMaster()
        {
            InvoiceWorkFlow = new HashSet<InvoiceWorkFlow>();
        }

        public Guid WorkFlowMasterId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<InvoiceWorkFlow> InvoiceWorkFlow { get; set; }
    }
}
