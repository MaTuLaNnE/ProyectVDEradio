using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectVDEradio.ViewModels
{
    public class EditRadioProgramViewModel
    {
        public int ProgramId { get; set; }

        [Required(ErrorMessage = "El nombre del programa es obligatorio")]
        [Display(Name = "Nombre del Programa")]
        public string ProgramName { get; set; }

        [Display(Name = "Imagen (URL)")]
        public string Image { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Debe especificar la hora de inicio")]
        [Display(Name = "Hora de Inicio")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Debe especificar la hora de finalización")]
        [Display(Name = "Hora de Finalización")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Días de emisión")]
        public List<int> SelectedDays { get; set; }

        [Display(Name = "Conductores")]
        public List<int> SelectedHostIds { get; set; }

        public List<SelectListItem> HostsDisponibles { get; set; }

        public EditRadioProgramViewModel()
        {
            SelectedDays = new List<int>();
            SelectedHostIds = new List<int>();
            HostsDisponibles = new List<SelectListItem>();
        }
    }
}