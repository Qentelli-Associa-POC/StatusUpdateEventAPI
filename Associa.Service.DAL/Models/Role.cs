using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class Role
    {
        public Role()
        {
            Person = new HashSet<Person>();
            TemplateStep = new HashSet<TemplateStep>();
            TemplateStore = new HashSet<TemplateStore>();
        }

        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<Person> Person { get; set; }
        public virtual ICollection<TemplateStep> TemplateStep { get; set; }
        public virtual ICollection<TemplateStore> TemplateStore { get; set; }
    }
}
