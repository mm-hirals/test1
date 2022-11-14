using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.Dto.APIResponse
{
    public class Permissions
    {
        public string Feature { get; set; }
        public bool HasAccess { get; set; }
    }
}
