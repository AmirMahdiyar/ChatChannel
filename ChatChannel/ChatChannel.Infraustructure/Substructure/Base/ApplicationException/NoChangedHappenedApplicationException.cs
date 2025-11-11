using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Infraustructure.Substructure.Base.ApplicationException
{
    public class NoChangedHappenedApplicationException : BaseException.ApplicationException
    {
        public NoChangedHappenedApplicationException() { }
        public NoChangedHappenedApplicationException(string message) : base(message) { }
        public NoChangedHappenedApplicationException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}
