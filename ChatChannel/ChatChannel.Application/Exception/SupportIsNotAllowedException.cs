using ChatChannel.Infraustructure.Substructure.Base.ApplicationException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Application.Exception
{
    public class SupportIsNotAllowedException : NotAllowedApplicationException
    {
        public SupportIsNotAllowedException() : base("Support Role Is Not Allowed To Use This Method") { }
        
    }
}
