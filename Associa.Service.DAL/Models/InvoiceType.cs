using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class InvoiceType
    {
        public InvoiceType()
        {
            Invoice = new HashSet<Invoice>();
            TemplateStore = new HashSet<TemplateStore>();
            WorkFlowStatus = new HashSet<WorkFlowStatus>();
        }

        public Guid InvoiceTypeId { get; set; }
        public string Name { get; set; }
        public string InvoiceCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<TemplateStore> TemplateStore { get; set; }
        public virtual ICollection<WorkFlowStatus> WorkFlowStatus { get; set; }
    }
}
