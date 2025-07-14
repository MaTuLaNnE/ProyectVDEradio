using ProyectVDEradio.Models;
using ProyectVDEradio.Utils;
using ProyectVDEradio.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProyectVDEradio.Controllers
{
    public class SponsorsController : Controller
    {
        private VozDelEsteDBEntities db = new VozDelEsteDBEntities();

        [AuthorizePermiso("SponsorsView")]
        // GET: Sponsor
        public ActionResult Index()
        {
            return View(db.Sponsors.ToList());
        }

        // GET: Sponsor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sponsor = db.Sponsors.Find(id);
            if (sponsor == null)
                return HttpNotFound();

            return View(sponsor);
        }

        // GET: Sponsor/Create
        [AuthorizePermiso("SponsorsCreate")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sponsors vm)
        {
            if (ModelState.IsValid)
            {
                var sponsor = new Sponsors
                {
                    SponsorName = vm.SponsorName,
                    SponsorDescription = vm.SponsorDescription,
                    SponsorPlan = vm.SponsorPlan,
                    SponsorImage = vm.SponsorImage
                };

                db.Sponsors.Add(sponsor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        // GET: Sponsor/Edit/5
        [AuthorizePermiso("SponsorsEdit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sponsor = db.Sponsors.Find(id);
            if (sponsor == null)
                return HttpNotFound();

            return View(sponsor);
        }

        // POST: Sponsor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizePermiso("SponsorsEdit")]
        public ActionResult Edit(Sponsors sponsor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sponsor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sponsor);
        }

        // GET: Sponsor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sponsor = db.Sponsors.Find(id);
            if (sponsor == null)
                return HttpNotFound();

            return View(sponsor);
        }

        // POST: Sponsor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizePermiso("SponsorsDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var sponsor = db.Sponsors.Find(id);
            db.Sponsors.Remove(sponsor);
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