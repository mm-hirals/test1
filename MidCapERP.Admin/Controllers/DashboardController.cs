﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.Admin.Models;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto;
using NToastNotify;
using System.Diagnostics;

namespace MidCapERP.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly ILogger<DashboardController> _logger;
        private readonly CurrentUser _currentUser;
        private readonly IToastNotification _toastNotification;

        public DashboardController(IUnitOfWorkBL unitOfWorkBL, ILogger<DashboardController> logger, CurrentUser currentUser, IToastNotification toastNotification, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _currentUser = currentUser;
            _logger = logger;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Dashboard.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            try
            {
                var orderCount = await _unitOfWorkBL.DashboardBL.GetOrderCount(cancellationToken);
                ViewBag.OrderCount = orderCount;

                var customerCount = await _unitOfWorkBL.DashboardBL.GetCustomerCount(cancellationToken);
                ViewBag.CustomerCount = customerCount;

                var interiorCount = await _unitOfWorkBL.DashboardBL.GetInteriorCount(cancellationToken);
                ViewBag.InteriorCount = interiorCount;

                var productCount = await _unitOfWorkBL.DashboardBL.GetProductCount(cancellationToken);
                ViewBag.ProductCount = productCount;
                return View();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Dashboard.View)]
        public IActionResult IndexNew(CancellationToken cancellationToken)
        {
            return View();
        }

        public IActionResult Privacy(CancellationToken cancellationToken)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(CancellationToken cancellationToken)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("PageNotFoundError")]
        public async Task<IActionResult> PageNotFoundError(CancellationToken cancellationToken)
        {
            return View("PageNotFoundError");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("InternalServerError")]
        public async Task<IActionResult> InternalServerError(CancellationToken cancellationToken)
        {
            return View("InternalServerError");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("BadRequestError")]
        public async Task<IActionResult> BadRequestError(CancellationToken cancellationToken)
        {
            return View("BadRequestError");
        }
    }
}