using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto;
using MidCapERP.Dto.Order;
using MidCapERP.Dto.Product;
using MidCapERP.Dto.Tenant;

namespace MidCapERP.Admin.Controllers
{
    public class DetailController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly CurrentUser _currentUser;
        public readonly IMapper _mapper;

        public DetailController(IUnitOfWorkBL unitOfWorkBL, CurrentUser currentUser, IMapper mapper)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/ProductDetail/{id}")]
        public async Task<IActionResult> ProductDetail(int Id, CancellationToken cancellationToken)
        {
            var data = _mapper.Map<ProductdetailAnonymousResponseDto>(await _unitOfWorkBL.ProductBL.GetProductDetailById(Id, cancellationToken));
            data.TenantResponseDto = await _unitOfWorkBL.TenantBL.GetById(_currentUser.TenantId, cancellationToken);
            return View("ProductDetail", data);
        }
    }
}