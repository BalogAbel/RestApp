using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Model
{
    public interface IVersioned
    {
        Guid RowVersion { get; set; }
    }
}