﻿using Com.Ambassador.Service.Inventory.Lib.Services;
using Com.Ambassador.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.Report.Bookkeeping;
using Com.Ambassador.Service.Inventory.Lib.ViewModels.GarmentLeftoverWarehouse.Report.Bookkeeping;
using Com.Ambassador.Service.Inventory.WebApi.Controllers.v1.GarmentLeftoverWarehouse.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Com.Ambassador.Service.Inventory.Test.Controllers.GarmentLeftoverWarehouse.Report
{
    public class GarmentLeftoverWarehouseStockBookkeepingReportControllerTest
    {
        protected (Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IGarmentLeftoverWarehouseStockBookkeepingReportService> Service) GetMocks()
        {
            return (IdentityService: new Mock<IIdentityService>(), ValidateService: new Mock<IValidateService>(), Service: new Mock<IGarmentLeftoverWarehouseStockBookkeepingReportService>());
        }

        protected GarmentLeftoverWarehouseStockBookkeepingReportController GetController((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IGarmentLeftoverWarehouseStockBookkeepingReportService> Service) mocks)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentLeftoverWarehouseStockBookkeepingReportController controller = new GarmentLeftoverWarehouseStockBookkeepingReportController(mocks.IdentityService.Object, mocks.ValidateService.Object, mocks.Service.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            return controller;
        }

        protected int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        //private int GetStatusCodeGet((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IGarmentLeftoverWarehouseStockBookkeepingReportService> Service) mocks)
        //{
        //    GarmentLeftoverWarehouseStockBookkeepingReportController controller = GetController(mocks);
        //    IActionResult response = controller.Get();

        //    return GetStatusCode(response);
        //}

        [Fact]
        public void Should_Success_GetReport()
        {
            var mocks = GetMocks();

            mocks.Service.Setup(f => f.GetReport(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), 7))
                .Returns(new Tuple<List<GarmentLeftoverWarehouseStockBookkeepingReportViewModel>, int>(new List<GarmentLeftoverWarehouseStockBookkeepingReportViewModel>(), 1));


            var response = GetController(mocks).Get(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), "");
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));

        }
        [Fact]
        public void Should_Error_GetReport()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReport(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), 7))
                .Throws(new Exception());
            var controller = GetController(mocks);
            controller.ControllerContext.HttpContext.Request.Headers["Accept"] = "application/json";
            var response = GetController(mocks).Get( It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), "");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Should_Success_GetXlsReport()
        {
            var mocks = GetMocks();

            mocks.Service.Setup(f => f.GenerateExcel(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>())
               ).Returns(new Tuple<MemoryStream, string>(It.IsAny<MemoryStream>(), It.IsAny<string>()));
            var response = GetController(mocks).Get(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());
            Assert.NotNull(response);

        }

        [Fact]
        public void Should_Error_GetXlsReport()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcel(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>())
            ).Throws(new Exception());
            var controller = GetController(mocks);
            controller.ControllerContext.HttpContext.Request.Headers["Accept"] = "application/json";
            var response = GetController(mocks).GetXlsAll( It.IsAny<DateTime>(), It.IsAny<string>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
    }
}
