﻿using Com.Ambassador.Service.Inventory.Lib;
using Com.Ambassador.Service.Inventory.Lib.Helpers;
using Com.Ambassador.Service.Inventory.Lib.Models.InventoryWeavingModel;
using Com.Ambassador.Service.Inventory.Lib.ViewModels.InventoryWeavingViewModel.Report;
using Com.Ambassador.Service.Inventory.Lib.Services.InventoryWeaving.Reports.ReportGreigeWeavingPerType;
using Com.Ambassador.Service.Inventory.Lib.Services.InventoryWeaving;
using Com.Ambassador.Service.Inventory.Lib.Services.InventoryWeaving.Reports.ReportGreigeWeavingPerGrade;
using Com.Ambassador.Service.Inventory.Test.DataUtils.InventoryWeavingDataUtils.ReportGreigeWeavingPerGradeDataUtil;
using Com.Ambassador.Service.Inventory.Test.DataUtils.InventoryWeavingDataUtils;
using Com.Ambassador.Service.Inventory.Test.Helpers;
using Com.Moonlay.NetCore.Lib;
using Com.Ambassador.Service.Inventory.Lib.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Com.Ambassador.Service.Inventory.Test.Services.InventoryWeaving.Report.ReportGreigeWeavingPerType
{
    public class ReportGreigeWeavingPerTypeTest
    {
        private const string ENTITY = "ReportPerType";
        //private string username;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private InventoryDbContext _dbContext(string testName)
        {
            DbContextOptionsBuilder<InventoryDbContext> optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            InventoryDbContext dbContext = new InventoryDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private ReportGreigeWeavingPerGradeDataUtil _dataUtil(ReportGreigeWeavingPerGradeService service, InventoryWeavingDocumentDataUtils dataDoc)
        {
            GetServiceProvider();
            return new ReportGreigeWeavingPerGradeDataUtil(service, dataDoc);
        }

        private InventoryWeavingDocumentDataUtils _dataUtilDoc(InventoryWeavingDocumentUploadService service)
        {
            GetServiceProvider();
            return new InventoryWeavingDocumentDataUtils(service);
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });

            return serviceProvider;
        }

        private Mock<IServiceProvider> GetFailServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpService)))
                .Returns(new HttpFailTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });


            return serviceProvider;
        }

        [Fact]
        public async Task Should_success_GetReport()
        {
            ReportGreigeWeavingPerGradeService service = new ReportGreigeWeavingPerGradeService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            InventoryWeavingDocumentUploadService serviceDoc = new InventoryWeavingDocumentUploadService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            InventoryWeavingDocumentDataUtils dataDoc1 = new InventoryWeavingDocumentDataUtils(serviceDoc);

            var Utilservice = new ReportGreigeWeavingPerTypeService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = _dataUtil(service, dataDoc1).GetTestData();
            
            var dataDoc = _dataUtilDoc(serviceDoc).GetTestData();
            //var Responses =  Utilservice.Create(data);

            var Service = new InventoryWeavingMovementService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var Response = Utilservice.GetStockReport(DateTime.UtcNow, 7, 25, 1, "{}");
            Assert.NotNull(Response);
        }

        [Fact]
        public void Should_success_GenerateExcel()
        {
            ReportGreigeWeavingPerGradeService service = new ReportGreigeWeavingPerGradeService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            InventoryWeavingDocumentUploadService serviceDoc = new InventoryWeavingDocumentUploadService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            InventoryWeavingDocumentDataUtils dataDoc1 = new InventoryWeavingDocumentDataUtils(serviceDoc);

            var Utilservice = new ReportGreigeWeavingPerTypeService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = _dataUtil(service, dataDoc1).GetTestData();
            
            var dataDoc = _dataUtilDoc(serviceDoc).GetTestData();
            //var Responses =  Utilservice.Create(data);

            var Service = new ReportGreigeWeavingPerGradeService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var Response = Utilservice.GenerateExcel(DateTime.UtcNow, 7);
            Assert.IsType<System.IO.MemoryStream>(Response);
        }
    }
}
