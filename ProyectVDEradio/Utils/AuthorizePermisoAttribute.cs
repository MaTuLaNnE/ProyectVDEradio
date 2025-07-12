using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;

namespace ProyectVDEradio.Utils
{
    public class AuthorizePermisoAttribute : AuthorizeAttribute
    {
        private readonly string _permiso;

        public AuthorizePermisoAttribute(string permiso)
        {
            _permiso = permiso;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User as ClaimsPrincipal;

            // Si no está autenticado, fuera
            if (user == null || !user.Identity.IsAuthenticated)
                return false;

            // Revisa si tiene el claim de permiso
            return user.HasClaim("Permiso", _permiso);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext); // Redirige a Login
            }
            else
            {
                // Si está logueado pero no tiene permiso => 403
                filterContext.Result = new HttpStatusCodeResult(403);
            }
        }
    }
}