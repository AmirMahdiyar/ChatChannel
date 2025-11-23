using ChatChannel.Infraustructure.Substructure.Base.ApplicationException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Application.Exception
{
    public class CustomerIsNotAllowedException : NotAllowedApplicationException
    {
        public CustomerIsNotAllowedException() : base("Customer Role Is Not Allowed To Use This Method") { }
    }
}
