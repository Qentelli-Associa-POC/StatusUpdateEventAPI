using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class PersonHoaMapping
    {
        public Guid PersonHoaMappingId { get; set; }
        public Guid HoaId { get; set; }
        public Guid PersonId { get; set; }
        public bool IsActive { get; set; }

        public virtual Hoa Hoa { get; set; }
        public virtual Person Person { get; set; }
    }
}
