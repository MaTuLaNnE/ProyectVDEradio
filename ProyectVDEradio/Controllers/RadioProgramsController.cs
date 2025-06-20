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
                            .Include(p => p.ProgramDays)
                            .Include(p => p.ProgramHosts.Select(ph => ph.Hosts))
                            .Include(p => p.CustomersComments.Select(c => c.Customers))
                            .ToList() // Necesario para evaluar TimeSpan correctamente en C#
                            .Where(p => p.ProgramDays.Any(d => d.WeekDay == currentDay))
                            .FirstOrDefault(p =>
                                (p.StartTime < p.EndTime && currentTime >= p.StartTime && currentTime < p.EndTime) || // dentro del mismo día
                                (p.StartTime > p.EndTime && (currentTime >= p.StartTime || currentTime < p.EndTime))   // cruza medianoche
                             );



            int? idProgramaActual = programaActual?.ProgramId;

            var todosMenosElActual = db.RadioPrograms
                .Where(p => idProgramaActual == null || p.ProgramId != idProgramaActual)
                .OrderByDescending(p => p.StartTime)
                .ToList();




            var model = new RadioProgramsViewModel
            {
                ListaRadios = todosMenosElActual,
                RadioActual = programaActual,
                Conductores = programaActual?.ProgramHosts
                     .Select(ph => ph.Hosts)
                     .ToList() ?? new List<Hosts>()

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(int ProgramId, string CustomerName, string Comment)
        {
            try
            {
                // Validar que los datos no estén vacíos
                if (string.IsNullOrWhiteSpace(CustomerName) || string.IsNullOrWhiteSpace(Comment))
                {
                    TempData["Error"] = "Por favor completa todos los campos.";
                    return RedirectToAction("Index"); // o la acción que corresponda
                }

                // Validar longitud
                if (CustomerName.Length > 50 || Comment.Length > 500)
                {
                    TempData["Error"] = "El nombre o comentario son demasiado largos.";
                    return RedirectToAction("Index");
                }

                // Buscar o crear el cliente
                var customer = db.Customers.FirstOrDefault(c => c.CustomerName == CustomerName.Trim());
                if (customer == null)
                {
                    customer = new Customers
                    {
                        CustomerName = CustomerName.Trim(),
                        // Agrega otros campos si los tienes
                    };
                    db.Customers.Add(customer);
                    db.SaveChanges();
                }

                // Crear el comentario
                var newComment = new CustomersComments
                {
                    ProgramId = ProgramId,
                    CustomerId = customer.CustomerId, // Asegúrate del nombre correcto de la propiedad
                    Comment = Comment.Trim(),
                    CommentDate = DateTime.Now
                };

                db.CustomersComments.Add(newComment);
                db.SaveChanges();

                TempData["Success"] = "¡Comentario enviado exitosamente!";
            }
            catch (Exception ex)
            {
                // Log del error si tienes sistema de logging
                TempData["Error"] = "Ocurrió un error al enviar el comentario. Inténtalo de nuevo.";
            }

            return RedirectToAction("Index"); // o la acción que corresponda
        }


    }
}
