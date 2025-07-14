using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectVDEradio.ViewModels
{
    public class CreateProgramViewModel
    {
        [Required]
        public string ProgramName { get; set; }

        public string Image { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Hora de Inicio")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "Hora de Fin")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        // Para checkboxes de días
        public List<int> SelectedDays { get; set; }

        // Para multiselect de conductores
        public List<int> SelectedHostIds { get; set; }

        public IEnumerable<SelectListItem> HostsDisponibles { get; set; }
    }
}