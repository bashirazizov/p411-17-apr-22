using AznConverterApi.cbar;
using AznConverterApi.DAL;
using AznConverterApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AznConverterApi.Services
{
    public class CurrencyService
    {
        private readonly AppDbContext db;
        public CurrencyService(AppDbContext _db)
        {
            db = _db;
        }

        public void ActualizeData()
        {
            CbarLog lastLog = db.CbarLogs.OrderByDescending(x => x.Id).FirstOrDefault();
            if (lastLog == null || lastLog.LastGet.ToString("ddMMyyyy") != DateTime.Now.ToString("ddMMyyyy"))
            {
                ValCurs vc = CbarService.GetCurrencyData();

                foreach (Valute item in vc.ValType[1].Valute)
                {
                    Currency c = db.Currencies.FirstOrDefault(x => x.Code == item.Code);
                    if (c != null)
                    {
                        c.Value = item.Value;
                        db.Update(c);
                        db.SaveChanges();
                    }
                    else
                    {
                        Currency newC = new Currency(item.Nominal, item.Name, item.Value, item.Code);
                        db.Currencies.Add(newC);
                        db.SaveChanges();
                    }
                }

                db.CbarLogs.Add(new CbarLog());
                db.SaveChanges();
            }
        }

        public decimal ConvertFromAzn(decimal amount, string code)
        {
            ActualizeData();
            Currency currency = db.Currencies.FirstOrDefault(x=>x.Code == code);
            return Math.Round(amount / Convert.ToDecimal(currency.Value), 2);
        }
        public decimal ConvertToAzn(decimal amount, string code)
        {
            ActualizeData();
            Currency currency = db.Currencies.FirstOrDefault(x => x.Code == code);
            return Math.Round(amount * Convert.ToDecimal(currency.Value),2);
        }

        public List<Currency> GetAllCurrencies()
        {
            return db.Currencies.ToList();
        }
    }
}
