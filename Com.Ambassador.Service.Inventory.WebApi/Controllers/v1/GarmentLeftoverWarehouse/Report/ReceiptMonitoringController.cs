﻿using Com.Ambassador.Service.Inventory.Lib.Services;
using Com.Ambassador.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.Report.Receipt;
using Com.Ambassador.Service.Inventory.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Ambassador.Service.Inventory.WebApi.Controllers.v1.GarmentLeftoverWarehouse.Report
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/garment/leftover-warehouse-receipts/monitoring")]
    [Authorize]
    public class ReceiptMonitoringController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly IReceiptMonitoringService Service;
        private readonly string ApiVersion;
        //private static readonly string ApiVersion = "1.0";
        //private MaterialsRequestNoteService materialsRequestNoteService { get; }

        public ReceiptMonitoringController(IIdentityService identityService, IValidateService validateService, IReceiptMonitoringService service)
        {
            IdentityService = identityService;
            ValidateService = validateService;
            Service = service;
            ApiVersion = "1.0.0";
        }

        protected void VerifyUser()
        {
            IdentityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            IdentityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            IdentityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpGet]
        public IActionResult Get(DateTime? dateFrom, DateTime? dateTo, string type, int page, int size, string Order = "{}")
        {
            int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
            string accept = Request.Headers["Accept"];

            try
            {
                VerifyUser();
                if (type == "FABRIC")
                {
                    var data = Service.GetFabricReceiptMonitoring(dateFrom, dateTo, page, size, Order, offset);

                    return Ok(new
                    {
                        apiVersion = ApiVersion,
                        data = data.Item1,
                        info = new { total = data.Item2 }
                    });
                }
                else
                {
                    var data = Service.GetAccessoriesReceiptMonitoring(dateFrom, dateTo, page, size, Order, offset);

                    return Ok(new
                    {
                        apiVersion = ApiVersion,
                        data = data.Item1,
                        info = new { total = data.Item2 }
                    });
                }
                
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("download")]
        public IActionResult GetXlsAll(DateTime? dateFrom, DateTime? dateTo, string type)
        {

            try
            {
                VerifyUser();
                byte[] xlsInBytes;
                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : Convert.ToDateTime(dateFrom);
                DateTime DateTo = dateTo == null ? DateTime.Now : Convert.ToDateTime(dateTo);

                if (type == "FABRIC")
                {
                    var generatedExcel = Service.GenerateExcelFabric(dateFrom, dateTo,  offset);

                    string filename = String.Format("Report Penerimaan Gudang Sisa Fabric - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));
                    //xlsInBytes = xls.ToArray();
                    //var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                    //return file;
                    return File(generatedExcel.Item1.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", generatedExcel.Item2);
                    
                }
                else
                {
                    var generatedExcel = Service.GenerateExcelAccessories(dateFrom, dateTo, offset);

                    string filename = String.Format("Report Pengeluaran Gudang Sisa Accessories- {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

                    return File(generatedExcel.Item1.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", generatedExcel.Item2);
                }

            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}
