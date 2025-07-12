using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectVDEradio.Controllers
{
    public class ManagementController : Controller
    {
        // GET: Management
        [Authorize(Roles = "Administrador,Editor")]
        public ActionResult Index()
        {

            return View();
        }
    }
}