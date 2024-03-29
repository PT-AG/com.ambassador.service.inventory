﻿using Com.Ambassador.Service.Inventory.Lib.Helpers;
using Com.Ambassador.Service.Inventory.Lib.ViewModels;
using Com.Ambassador.Service.Inventory.Test.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Ambassador.Service.Inventory.Test.DataUtils.IntegrationDataUtil
{
    public class StorageDataUtil
    {
        public static StorageViewModel GetPrintingGreigeStorage(HttpClientTestService client)
        {
            Dictionary<string, object> filter = new Dictionary<string, object> { { "name", "Gudang Greige Printing" } };
            var response = client.GetAsync($@"{APIEndpoint.Core}master/storages?filter=" + JsonConvert.SerializeObject(filter)).Result.Content.ReadAsStringAsync();
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Result);

            List<StorageViewModel> list = JsonConvert.DeserializeObject<List<StorageViewModel>>(result["data"].ToString());
            StorageViewModel storage = list.First();

            return storage;
        }
    }
}
