using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectVDEradio.Models;
using System.Data.Entity;
using ProyectVDEradio.ViewModels;



namespace ProyectVDEradio.Controllers
{
    public class HomeController : Controller
    {

        private VozDelEsteDBEntities db = new VozDelEsteDBEntities();

        public ActionResult Index()
        {
            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;
            var currentDay = (int)now.DayOfWeek;

            var modelo = new HomeViewModel
            {
                Noticias = db.News.Include(n => n.Categories)
                                  .OrderByDescending(n => n.ArticleDate)
                                  .Take(4)
                                  .ToList(),
                ProgramaEnVivo = db.RadioPrograms
                                   .Include(p => p.ProgramHosts)
                                   .Include(p => p.CustomersComments)
                                   .Where(p => db.ProgramDays
                                   .Any(d => d.ProgramId == p.ProgramId && d.WeekDay == currentDay) &&
                                   p.StartTime <= currentTime && p.EndTime >= currentTime)
                                   .FirstOrDefault(),
                ultimoProgramaEmitido = db.RadioPrograms
                                     .Include(p => p.ProgramHosts)
                                     .Include(p => p.CustomersComments)
                                     .Include(p => p.ProgramDays)
                                     .Where(p => p.StartTime < currentTime || (p.StartTime > p.EndTime && currentTime < p.EndTime)) 
                                     .OrderByDescending(p => p.StartTime)
                                     .FirstOrDefault()



            };

            return View(modelo);
        }


    }
}