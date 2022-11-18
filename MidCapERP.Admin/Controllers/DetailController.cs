using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var data = await _unitOfWorkBL.ProductBL.GetProductDetailById(Id, cancellationToken);
            return View("ProductDetail", data);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/OrderDetail/{orderNo}")]
        public async Task<IActionResult> OrderDetail(string orderNo, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.GetOrderDetailsAnonymous(orderNo, cancellationToken);
            if (data != null)
            {
                string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                data.IpAddress = ipAddress;
                await _unitOfWorkBL.OrderAnonymousBL.CreateOrderDetailsAnonymous(data, cancellationToken);
                return View("OrderDetail", data);
            }
            else
            {
                throw new Exception("Please enter valid order no!");
            }
        }
    }
}