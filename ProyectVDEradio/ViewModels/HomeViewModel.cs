using ProyectVDEradio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectVDEradio.ViewModels;
using ProyectVDEradio.Utils.WeatherAPI;

namespace ProyectVDEradio.ViewModels
{
    public class HomeViewModel
    {
        public List<News> Noticias { get; set; }
        public RadioPrograms ProgramaEnVivo { get; set; }
        public RadioPrograms ultimoProgramaEmitido { get; set; }
        WeatherData Weather {  get; set; }
    }
}