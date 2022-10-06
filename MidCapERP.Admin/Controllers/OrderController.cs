using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.Order;

namespace MidCapERP.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public OrderController(IStringLocalizer<BaseController> localizer, IUnitOfWorkBL unitOfWorkBL) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Order.View)]
        public IActionResult Index(CancellationToken cancellationToken)
        {
            FillRefferedDropDown(cancellationToken);
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.View)]
        public async Task<IActionResult> GetOrderData([FromForm] OrderDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.GetFilterOrderData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.View)]
        public async Task<IActionResult> OrderDetail(long Id, CancellationToken cancellationToken)
        {
            OrderResponseDto orderData = new OrderResponseDto();
            if (Id > 0)
            {
                orderData = await _unitOfWorkBL.OrderBL.GetOrderDetailData(Id, cancellationToken);
            }
            return View(orderData);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.View)]
        public async Task<IActionResult> CustomerDetail(long CustomerId, CancellationToken cancellationToken)
        {
            var customerById = await _unitOfWorkBL.CustomersBL.GetById(CustomerId, cancellationToken);
            return PartialView("Order_CustomerPartial", customerById);
        }

        #region Private Method

        private async void FillRefferedDropDown(CancellationToken cancellationToken)
        {
            var customerData = await _unitOfWorkBL.CustomersBL.GetAll(cancellationToken);

            var referedByDataSelectedList = customerData.Where(p => p.CustomerTypeId == (int)ArchitectTypeEnum.Architect).Select(
                                    p => new { p.CustomerId, p.FirstName,p.LastName }).Select(a =>
                                    new SelectListItem
                                    {
                                        Value = Convert.ToString(a.CustomerId),
                                        Text = Convert.ToString(a.FirstName+ " " +a.LastName)
                                    }).ToList();
            ViewBag.ReferedBySelectItemList = referedByDataSelectedList;
        }

        #endregion Private Method
    }
}