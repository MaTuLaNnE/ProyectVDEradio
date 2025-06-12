using ProyectVDEradio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectVDEradio.ViewModels
{
    public class RadioProgramsViewModel
    {

        public RadioProgramsViewModel()
        {
            ListaRadios = new List<RadioPrograms>();
            Conductores = new List<Hosts>();
            TodaySchedule = new List<TodayScheduleItem>();
            RelatedPrograms = new List<RadioPrograms>();
        }

        public List<RadioPrograms> ListaRadios { get; set; }
        public RadioPrograms RadioActual { get; set; }
        public List<Hosts> Conductores { get; set; }
        public List<TodayScheduleItem> TodaySchedule { get; set; }
        public RadioPrograms Program { get; set; } // Programa detallado actual
        public int WeeksOnAir { get; set; }
        public List<RadioPrograms> RelatedPrograms { get; set; }


    }
    public class TodayScheduleItem
    {
        public DateTime Time { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCurrentSegment { get; set; }
    }

}