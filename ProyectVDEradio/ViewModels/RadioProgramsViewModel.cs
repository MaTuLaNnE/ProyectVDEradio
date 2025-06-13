using ProyectVDEradio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectVDEradio.ViewModels
{
    public class RadioProgramsViewModel
    {


        public List<RadioPrograms> ListaRadios { get; set; }
        public RadioPrograms RadioActual { get; set; }
        public List<Hosts> Conductores { get; set; }

        
    }
}