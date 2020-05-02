using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class InvoiceTracker
    {
        public Guid InvoiceTrackerId { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid PersonId { get; set; }
        public bool CurrentStatus { get; set; }
        public string InvoiceFlowStatus { get; set; }
        public string OutcomeStatus { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public int? Sequence { get; set; }
        public string Status { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Person Person { get; set; }
    }
}
