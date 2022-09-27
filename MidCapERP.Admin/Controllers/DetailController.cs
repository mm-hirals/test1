using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;

namespace MidCapERP.Admin.Controllers
{
    public class DetailController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public DetailController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/ProductDetail/{id}")]
        public async Task<IActionResult> ProductDetail(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.ProductBL.GetById(Id, cancellationToken);
            return View("ProductDetail", data);
        }
    }
}
