using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.SharedKernel.Exceptions
{
    /// <summary>
    /// For handling DbUpdateConcurrencyException, because this is Infrastructure concerns and we should not use it in the application layer
    /// </summary>
    public class CustomConcurrencyException : Exception
    {
        public CustomConcurrencyException()
        { }

        public CustomConcurrencyException(string message)
            : base(message)
        { }

        public CustomConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
