using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class VendorHoaMapping
    {
        public Guid VendorHoaMappingId { get; set; }
        public Guid HoaId { get; set; }
        public Guid VendorId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public virtual Hoa Hoa { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
