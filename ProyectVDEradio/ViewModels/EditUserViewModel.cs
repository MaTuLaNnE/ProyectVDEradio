using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectVDEradio.ViewModels
{
    public class EditUserViewModel
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Rol")]
        public int UserRole { get; set; }

        // Campos solo si es cliente
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        public bool EsCliente => UserRole == 3; // o el ID del rol de Cliente

        public IEnumerable<SelectListItem> RolesDisponibles { get; set; }


    }
}