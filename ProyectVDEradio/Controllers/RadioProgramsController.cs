using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectVDEradio.Models;
using ProyectVDEradio.ViewModels;

namespace ProyectVDEradio.Controllers
{
    public class RadioProgramsController : Controller
    {
        private VozDelEsteDBEntities db = new VozDelEsteDBEntities();

        // GET: RadioPrograms
        public ActionResult Index()
        {

            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;
            var currentDay = (int)now.DayOfWeek;

            var programaActual = db.RadioPrograms
                .Where(p => db.ProgramDays
                .Any(d => d.ProgramId == p.ProgramId && d.WeekDay == currentDay) &&
                p.StartTime <= currentTime && p.EndTime >= currentTime)
                .FirstOrDefault();

            // 2. Obtener todos los programas, excluyendo el actual
            var todosMenosElActual = db.RadioPrograms
                .Where(p => p.ProgramId == null || p.ProgramId != programaActual.ProgramId)
                .OrderByDescending(p => p.StartTime)
                .ToList();

            List<Hosts> conductores = db.Hosts.OrderByDescending(ph => ph.HostName).ToList();


            var model = new RadioProgramsViewModel
            {
                ListaRadios = todosMenosElActual,
                RadioActual = programaActual,
                Conductores = conductores
            };

            return View(model);
        }

        // GET: RadioPrograms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RadioPrograms radioPrograms = db.RadioPrograms.Find(id);
            if (radioPrograms == null)
            {
                return HttpNotFound();
            }
            return View(radioPrograms);
        }

        // GET: RadioPrograms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RadioPrograms/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProgramId,ProgramName,Image,Description,StartTime,EndTime")] RadioPrograms radioPrograms)
        {
            if (ModelState.IsValid)
            {
                db.RadioPrograms.Add(radioPrograms);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(radioPrograms);
        }

        // GET: RadioPrograms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RadioPrograms radioPrograms = db.RadioPrograms.Find(id);
            if (radioPrograms == null)
            {
                return HttpNotFound();
            }
            return View(radioPrograms);
        }

        // POST: RadioPrograms/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProgramId,ProgramName,Image,Description,StartTime,EndTime")] RadioPrograms radioPrograms)
        {
            if (ModelState.IsValid)
            {
                db.Entry(radioPrograms).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(radioPrograms);
        }

        // GET: RadioPrograms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RadioPrograms radioPrograms = db.RadioPrograms.Find(id);
            if (radioPrograms == null)
            {
                return HttpNotFound();
            }
            return View(radioPrograms);
        }

        // POST: RadioPrograms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RadioPrograms radioPrograms = db.RadioPrograms.Find(id);
            db.RadioPrograms.Remove(radioPrograms);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
