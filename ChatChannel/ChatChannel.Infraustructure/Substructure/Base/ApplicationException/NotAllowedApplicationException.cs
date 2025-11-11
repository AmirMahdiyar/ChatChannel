using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Infraustructure.Substructure.Base.ApplicationException
{
    public class NotAllowedApplicationException : BaseException.ApplicationException
    {
        public NotAllowedApplicationException()
        {
        }

        public NotAllowedApplicationException(string? message) : base(message)
        {
        }

        public NotAllowedApplicationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
