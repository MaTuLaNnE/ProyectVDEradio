using ProyectVDEradio.Models;
using ProyectVDEradio.Utils.WeatherForecastAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;


namespace ProyectVDEradio.ViewModels
{
    public class WeatherViewModel
    {
        public WeatherViewModel()
        {
            Weather = new WeatherData();
        }

        public WeatherData Weather { get; set; }

        public List<News> noticias { get; set; }

        public List<ForecastData> Forecast { get; set; }

        public Currency Currency { get; set; }
    }
    public class WeatherData
    {
        public double Temp { get; set; }
        public string Estado { get; set; }
        public string Icono { get; set; }
        public double TempMax { get; set; }
        public double TempMin { get; set; }
        public int Humedad { get; set; }
        public double Viento { get; set; }
        public int Presion { get; set; }
        public double Sensacion { get; set; }
        public long Amanecer { get; set; }
        public long Atardecer { get; set; }

    }
    public class ForecastData
    {
        public DateTime Fecha { get; set; }
        public double Temp { get; set; }
        public string Estado { get; set; }
        public string Icono { get; set; }
        public double TempMax { get; set; }
        public double TempMin { get; set; }
        public int Humedad { get; set; }
        public double Viento { get; set; }
        public int Presion { get; set; }
        public double Sensacion { get; set; }
    }
    public class WeatherAuditHistory
    {
        public int WeatherAuditId { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal Temp { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int Feels_like { get; set; }
        public int Temp_min { get; set; }
        public int Temp_max { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
    }
    //------------------ Currencies ------------------

    public class Currency
    {
        public string Source { get; set; }
        public long TimeStamp { get; set; }
        public decimal UYUBRL { get; set; }
        public decimal UYUUSD { get; set; }
        public decimal UYUARS { get; set; }
    }
    public class CurrencyAuditHistory
    {
        public int AuditId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal UYUUSD { get; set; }
        public decimal UYUARS { get; set; }
        public decimal UYUBRL { get; set; }
    }


}