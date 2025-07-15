using ProyectVDEradio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectVDEradio.ViewModels
{
    public class UsersViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public int UserRole { get; set; }
        public string RoleName { get; set; }

        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public DateTime? BirthDate { get; set; }
    }

}