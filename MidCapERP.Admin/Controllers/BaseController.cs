using Microsoft.AspNetCore.Mvc;
using MidCapERP.Admin.Middleware;

namespace MidCapERP.Admin.Controllers
{
    [TenantSelectionFilter]
    public class BaseController : Controller
    {
        public BaseController()
        {
        }
    }
}