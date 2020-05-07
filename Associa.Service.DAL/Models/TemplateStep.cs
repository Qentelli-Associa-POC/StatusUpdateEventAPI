using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class TemplateStep
    {
        public TemplateStep()
        {
            TemplateMapping = new HashSet<TemplateMapping>();
            WorkFlowStatus = new HashSet<WorkFlowStatus>();
        }

        public Guid TemplateStepId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string ProcedureName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? RoleId { get; set; }

        public virtual Person Owner { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<TemplateMapping> TemplateMapping { get; set; }
        public virtual ICollection<WorkFlowStatus> WorkFlowStatus { get; set; }
    }
}
