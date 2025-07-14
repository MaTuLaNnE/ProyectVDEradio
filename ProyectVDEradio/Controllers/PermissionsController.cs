using ProyectVDEradio.Models;
using ProyectVDEradio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectVDEradio.Controllers
{
    public class PermissionsController : Controller
    {

        private VozDelEsteDBEntities db = new VozDelEsteDBEntities();
        // GET: Permissions
        public ActionResult PermissionPanel()
        {
            var roles = db.Roles.ToList();
            var permisos = db.Permissions.ToList();
            var permisosAsignados = db.PermissionRole.ToList();

            var model = new List<PermissionRoleViewModel>();

            foreach (var rol in roles)
            {
                var permisosCheckbox = permisos.Select(p => new PermissionCheckbox
                {
                    PermissionId = p.PermissionId,
                    PermissionName = p.PermissionName,
                    EstaAsignado = permisosAsignados.Any(pr => pr.RoleId == rol.RoleId && pr.PermissionId == p.PermissionId)
                }).ToList();

                model.Add(new PermissionRoleViewModel
                {
                    RoleId = rol.RoleId,
                    RoleName = rol.RoleName,
                    Permisos = permisosCheckbox
                });
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult GuardarPermisos(string[] PermisosAsignados)
        {
            var relacionesActuales = db.PermissionRole.ToList();

            var permisosSeleccionados = new HashSet<string>(PermisosAsignados ?? new string[0]);

            var permisos = db.Permissions.ToList();
            var roles = db.Roles.ToList();

            foreach (var rol in roles)
            {
                foreach (var permiso in permisos)
                {
                    string clave = $"{rol.RoleId}-{permiso.PermissionId}";
                    var existe = relacionesActuales.FirstOrDefault(r => r.RoleId == rol.RoleId && r.PermissionId == permiso.PermissionId);

                    if (permisosSeleccionados.Contains(clave))
                    {
                        if (existe == null)
                        {
                            db.PermissionRole.Add(new PermissionRole
                            {
                                RoleId = rol.RoleId,
                                PermissionId = permiso.PermissionId
                            });
                        }
                    }
                    else
                    {
                        if (existe != null)
                        {
                            db.PermissionRole.Remove(existe);
                        }
                    }
                }
            }

            db.SaveChanges();
            return RedirectToAction("PermissionPanel");
        }

    }
}