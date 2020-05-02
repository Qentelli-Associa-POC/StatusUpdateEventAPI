using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatusUpdateEventAPI.Helpers
{
    public class ModelValidationException : Exception
    {
        public ModelValidationException() : base()
        {

        }
        public ModelValidationException(string message) : base(message)
        {

        }
        public ModelValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
