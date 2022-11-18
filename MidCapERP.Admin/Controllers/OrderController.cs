using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.Order;
using MidCapERP.Dto.OrderSetItem;

namespace MidCapERP.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public OrderController(IStringLocalizer<BaseController> localizer, IUnitOfWorkBL unitOfWorkBL) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.PortalOrder.View)]
        public IActionResult Index(CancellationToken cancellationToken)
        {
            FillRefferedDropDown(cancellationToken);
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalOrder.View)]
        public async Task<IActionResult> GetOrderData([FromForm] OrderDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.GetFilterOrderData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalOrder.View)]
        public async Task<IActionResult> OrderDetail(long Id, CancellationToken cancellationToken)
        {
            OrderResponseDto data = new OrderResponseDto();
            data.OrderId = Id;
            return View("OrderMain", data);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderBasicDetail(int orderId, CancellationToken cancellationToken)
        {
            if (orderId > 0)
            {
                var getOrderInfoById = await _unitOfWorkBL.OrderBL.GetOrderDetailData(orderId, cancellationToken);
                return PartialView("_OrderBasicDetail", getOrderInfoById);
            }
            else
                throw new Exception("Order Id can not be null");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalOrder.View)]
        public async Task<IActionResult> GetOrderSetDetailData(long orderId, CancellationToken cancellationToken)
        {
            if (orderId > 0)
            {
                var orderSetData = await _unitOfWorkBL.OrderBL.GetOrderSetDetailData(orderId, cancellationToken);
                return PartialView("_OrderSetDetailPartial", orderSetData);
            }
            else
                throw new Exception("Order Id can not be null");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalOrder.View)]
        public async Task<IActionResult> SaveDiscount([FromForm] OrderSetItemRequestDto orderSetItemRequestDto, CancellationToken cancellationToken)
        {
            var orderSetItem = await _unitOfWorkBL.OrderBL.UpdateOrderSetItemDiscount(orderSetItemRequestDto, cancellationToken);

            var orderSetData = await _unitOfWorkBL.OrderBL.GetOrderSetDetailData(orderSetItem.OrderId, cancellationToken);
            return PartialView("_OrderSetDetailPartial", orderSetData);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalOrder.Create)]
        public async Task<JsonResult> ApproveOrderStatus(long Id, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _unitOfWorkBL.OrderBL.ApproveOrderStatus(Id, cancellationToken);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json("error");
            }
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalOrder.Create)]
        public async Task<JsonResult> DeclineOrderStatus(OrderResponseDto model, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWorkBL.OrderBL.DeclineOrderStatus(model, cancellationToken);
                return Json("success");
            }
            catch (Exception ex)
            {
                return Json("error");
            }
        }

        #region Private Method

        private async void FillRefferedDropDown(CancellationToken cancellationToken)
        {
            var customerData = await _unitOfWorkBL.CustomersBL.GetAll(cancellationToken);

            var referedByDataSelectedList = customerData.Where(p => p.CustomerTypeId == (int)CustomerTypeEnum.Interior || p.CustomerTypeId == (int)CustomerTypeEnum.Customer).Select(
                                    p => new { p.CustomerId, p.FirstName, p.LastName }).Select(a =>
                                    new SelectListItem
                                    {
                                        Value = Convert.ToString(a.CustomerId),
                                        Text = Convert.ToString(a.FirstName + " " + a.LastName)
                                    }).ToList();
            ViewBag.ReferedBySelectItemList = referedByDataSelectedList;
        }

        #endregion Private Method
    }
}