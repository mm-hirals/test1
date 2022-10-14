using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto;
using MidCapERP.Dto.AccessoriesType;
using MidCapERP.Dto.Product;

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
            var data = await _unitOfWorkBL.ProductBL.GetProductDetailById(Id, cancellationToken);
            return View("ProductDetail", data);
        }
    }
}