using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectVDEradio.ViewModels
{
    public class PermissionRoleViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public List<PermissionCheckbox> Permisos { get; set; }
    }

    public class PermissionCheckbox
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool EstaAsignado { get; set; }
    }
}