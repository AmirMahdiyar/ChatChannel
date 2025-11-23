using ChatChannel.Infraustructure.Substructure.Base.ApplicationException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Application.Exception
{
    public class RoleNotFoundException : NotFoundApplicationException
    {
        public RoleNotFoundException() : base("Role Can't Be Found In User Claims"){ }
    }
}
