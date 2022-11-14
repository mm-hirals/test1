using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.Dto.APIResponse
{
    public class PermissionsAPIResponse
    {
        public string ModuleName { get; set; }
        public List<Permissions> Permissions { get; set; } = new List<Permissions>();
    }
}
