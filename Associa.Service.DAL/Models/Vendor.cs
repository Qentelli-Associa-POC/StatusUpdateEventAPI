using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class Vendor
    {
        public Vendor()
        {
            Invoice = new HashSet<Invoice>();
            VendorHoaMapping = new HashSet<VendorHoaMapping>();
        }

        public Guid VendorId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public virtual Person VendorNavigation { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<VendorHoaMapping> VendorHoaMapping { get; set; }
    }
}
