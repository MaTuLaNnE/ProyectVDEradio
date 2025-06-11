using ProyectVDEradio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectVDEradio.ViewModels
{
    public class HomeViewModel
    {
        public List<News> Noticias { get; set; }
        public RadioPrograms ProgramaEnVivo { get; set; }
    }
}