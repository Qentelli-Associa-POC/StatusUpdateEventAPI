using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class Person
    {
        public Person()
        {
            InvoiceTracker = new HashSet<InvoiceTracker>();
            TemplateStep = new HashSet<TemplateStep>();
        }

        public Guid PersonId { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Guid RoleId { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public virtual Role Role { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<InvoiceTracker> InvoiceTracker { get; set; }
        public virtual ICollection<TemplateStep> TemplateStep { get; set; }
    }
}
