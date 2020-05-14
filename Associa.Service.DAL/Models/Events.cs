using System;
using System.Collections.Generic;

namespace Associa.Service.DAL.Models
{
    public partial class Events
    {
        public Events()
        {
            EventTransactionMapping = new HashSet<EventTransactionMapping>();
        }

        public Guid Id { get; set; }
        public int Sequence { get; set; }
        public string Code { get; set; }
        public string Procedure { get; set; }

        public virtual ICollection<EventTransactionMapping> EventTransactionMapping { get; set; }
    }
}
