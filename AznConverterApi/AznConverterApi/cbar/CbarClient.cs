using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AznConverterApi.cbar
{
    public static class CbarClient
    {
        public static string GetCurrencyRawData()
        {
            HttpClient client = new HttpClient();
            return client.GetStringAsync("https://www.cbar.az/currencies/" + DateTime.Now.ToString("dd.MM.yyyy") + ".xml").Result;
        }
    }
}
