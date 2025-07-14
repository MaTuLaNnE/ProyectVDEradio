using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProyectVDEradio.Utils.WeatherAPI;
using ProyectVDEradio.Utils.WeatherForecastAPI;

namespace ProyectVDEradio.Controllers
{
    public class WeatherController : Controller
    {

        public async Task<JsonResult> GetWeather()
        {
            string url = "https://api.openweathermap.org/data/2.5/weather?lat=-34.90&lon=-54.95&appid=7d3b86d0d678a4b70d2a0d0d027d78f1&units=metric&lang=es";


            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                var clima = Welcome.FromJson(response);

                var resultado = new
                {
                    temp = clima.Main.Temp,
                    estado = clima.Weather[0].Description,
                    icono = clima.Weather[0].Icon,
                    tempMax = clima.Main.TempMax,
                    tempMin = clima.Main.TempMin,
                    humedad = clima.Main.Humidity,
                    viento = clima.Wind.Speed,
                    presion = clima.Main.Pressure,
                    sensacion = clima.Main.FeelsLike,
                    amanecer = clima.Sys.Sunrise,
                    atardecer = clima.Sys.Sunset

                };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }


    }
}