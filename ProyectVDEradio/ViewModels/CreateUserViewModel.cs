using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectVDEradio.ViewModels
{
    public class CreateUserViewModel
    {
        // Campos de Users
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserPassword { get; set; }
        public int UserRole { get; set; }

        // Campos extra si el rol es cliente
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        // Para el dropdown de roles
        public IEnumerable<SelectListItem> RolesDisponibles { get; set; }

    }
}