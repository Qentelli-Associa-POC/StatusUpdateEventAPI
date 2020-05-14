using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class InvoiceWorkflowStatus
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Guid EventId { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        public long Sequence { get; set; }
    }
}
