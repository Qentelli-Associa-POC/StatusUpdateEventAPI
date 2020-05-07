using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class Hoa
    {
        public Hoa()
        {
            Invoice = new HashSet<Invoice>();
            PersonHoaMapping = new HashSet<PersonHoaMapping>();
            VendorHoaMapping = new HashSet<VendorHoaMapping>();
        }

        public Guid HoaId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<PersonHoaMapping> PersonHoaMapping { get; set; }
        public virtual ICollection<VendorHoaMapping> VendorHoaMapping { get; set; }
    }
}
