﻿using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class TemplateStore
    {
        public TemplateStore()
        {
            TemplateMapping = new HashSet<TemplateMapping>();
        }

        public Guid TemplateStoreId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public double? Amount { get; set; }
        public Guid InvoiceTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string UploadedBy { get; set; }
        public Guid? RoleId { get; set; }
        public string Segment { get; set; }
        public string Parameter { get; set; }

        public virtual InvoiceType InvoiceType { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<TemplateMapping> TemplateMapping { get; set; }
    }
}
