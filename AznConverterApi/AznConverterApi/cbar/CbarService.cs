using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AznConverterApi.cbar
{
    public static class CbarService
    {
        public static ValCurs GetCurrencyData()
        {
            string responseFromCbar = CbarClient.GetCurrencyRawData();
            StringReader sr = new StringReader(responseFromCbar);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ValCurs));
            ValCurs data = (ValCurs)xmlSerializer.Deserialize(sr);
            return data;
        }
    }
}
