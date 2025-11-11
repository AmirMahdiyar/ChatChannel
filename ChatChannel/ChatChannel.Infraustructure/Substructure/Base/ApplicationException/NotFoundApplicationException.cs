using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Infraustructure.Substructure.Base.ApplicationException
{
    public class NotFoundApplicationException : BaseException.ApplicationException
    {
        public NotFoundApplicationException() { }
        public NotFoundApplicationException(string message) : base(message) { }
        public NotFoundApplicationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
