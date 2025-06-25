using ProyectVDEradio.Utils;
using ProyectVDEradio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProyectVDEradio.Controllers
{
    public class CurrencyAPIController : Controller
    {
        // GET: CurrencyAPI
        public async Task<ActionResult> CurrencyAPI()
        {

            string urlCurrency = "http://apilayer.net/api/live?access_key=3b51394b74c5ace5216edbb5731c5961&currencies=BRL,USD,ARS&source=UYU&format=1";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(urlCurrency);
                var currency = Utils.CurrencyApi.FromJson(response);

                var curren = new Currency
                {
                    Source = currency.Source,
                    UYUBRL = currency.Quotes.Uyubrl ?? 0,
                    UYUARS = currency.Quotes.Uyuars ?? 0,
                    UYUUSD = currency.Quotes.Uyuusd ?? 0,
                    TimeStamp = currency.Timestamp ?? 0




                };





                return View(curren);
            }
        }
    }
}