using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceTracker = new HashSet<InvoiceTracker>();
            WorkFlowStatus = new HashSet<WorkFlowStatus>();
        }

        public Guid InvoiceId { get; set; }
        public string Title { get; set; }
        public Guid InvoiceTypeId { get; set; }
        public double? Amount { get; set; }
        public Guid HoaId { get; set; }
        public bool IsActive { get; set; }
        public Guid VendorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string Status { get; set; }

        public virtual Hoa Hoa { get; set; }
        public virtual InvoiceType InvoiceType { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<InvoiceTracker> InvoiceTracker { get; set; }
        public virtual ICollection<WorkFlowStatus> WorkFlowStatus { get; set; }
    }
}
