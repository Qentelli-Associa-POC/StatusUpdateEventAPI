using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class TemplateMapping
    {
        public Guid TemplateMappingId { get; set; }
        public Guid TemplateStoreId { get; set; }
        public Guid TemplateStepId { get; set; }
        public int? OrderId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public virtual TemplateStep TemplateStep { get; set; }
        public virtual TemplateStore TemplateStore { get; set; }
    }
}
