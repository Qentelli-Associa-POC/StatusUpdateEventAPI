using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class WorkFlowStatus
    {
        public Guid WorkFlowStatusId { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid InvoiceTypeId { get; set; }
        public Guid TemplateStepId { get; set; }
        public int SequenceId { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual InvoiceType InvoiceType { get; set; }
        public virtual TemplateStep TemplateStep { get; set; }
    }
}
