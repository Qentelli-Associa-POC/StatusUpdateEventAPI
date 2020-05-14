using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class EventTransactionMapping
    {
        public Guid Id { get; set; }
        public Guid HoaId { get; set; }
        public long Sequence { get; set; }
        public Guid WorkflowCategory { get; set; }
        public Guid EventId { get; set; }
        public bool? IsActive { get; set; }
        public string Parameter { get; set; }
        public string SegmentCondition { get; set; }
        public string WorkflowType { get; set; }
        public Guid? RoleId { get; set; }

        public virtual Events Event { get; set; }
        public virtual Hoa Hoa { get; set; }
        public virtual InvoiceType WorkflowCategoryNavigation { get; set; }
    }
}
