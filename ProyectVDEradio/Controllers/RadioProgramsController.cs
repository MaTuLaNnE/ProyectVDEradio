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
using ProyectVDEradio.Utils;
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
                            .ToList() 
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


        [AuthorizePermiso("ViewRadioPrograms")]
        public ActionResult RadioProgramsTable()
        {
            var programas = db.RadioPrograms
                           .Include(p => p.ProgramHosts.Select(ph => ph.Hosts))
                           .Include(p => p.ProgramDays)
                           .ToList();


            return View(programas);
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
        [AuthorizePermiso("CreateRadioPrograms")]
        public ActionResult Create()
        {
            var model = new CreateProgramViewModel
            {
                HostsDisponibles = db.Hosts.Select(h => new SelectListItem
                {
                    Value = h.HostId.ToString(),
                    Text = h.HostName
                }).ToList()
            };

            return View(model);
        }

        // POST: RadioPrograms/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizePermiso("CreateRadioPrograms")]
        public ActionResult Create(CreateProgramViewModel model)
        {
            if (ModelState.IsValid)
            {
                var nuevoPrograma = new RadioPrograms
                {
                    ProgramName = model.ProgramName,
                    Image = model.Image,
                    Description = model.Description,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime
                };

                db.RadioPrograms.Add(nuevoPrograma);
                db.SaveChanges();

                // Agregar días de emisión
                foreach (var dia in model.SelectedDays ?? new List<int>())
                {
                    db.ProgramDays.Add(new ProgramDays
                    {
                        ProgramId = nuevoPrograma.ProgramId,
                        WeekDay = dia
                    });
                }

                // Agregar hosts
                foreach (var hostId in model.SelectedHostIds ?? new List<int>())
                {
                    db.ProgramHosts.Add(new ProgramHosts
                    {
                        ProgramId = nuevoPrograma.ProgramId,
                        HostId = hostId
                    });
                }

                db.SaveChanges();
                return RedirectToAction("index", "Management");
            }

            // Si falla, volver a llenar dropdown
            model.HostsDisponibles = db.Hosts.Select(h => new SelectListItem
            {
                Value = h.HostId.ToString(),
                Text = h.HostName
            }).ToList();

            return View(model);
        }


        // GET: RadioPrograms/Edit/5
        [AuthorizePermiso("EditRadioPrograms")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var programa = db.RadioPrograms
                            .Include("ProgramDays")
                            .Include("ProgramHosts")
                            .FirstOrDefault(p => p.ProgramId == id);

            if (programa == null)
            {
                return HttpNotFound();
            }

            var model = new EditRadioProgramViewModel
            {
                ProgramId = programa.ProgramId,
                ProgramName = programa.ProgramName,
                Image = programa.Image,
                Description = programa.Description,
                StartTime = programa.StartTime,
                EndTime = programa.EndTime,
                SelectedDays = programa.ProgramDays.Select(d => d.WeekDay).ToList(),
                SelectedHostIds = programa.ProgramHosts.Select(h => h.HostId).ToList(),
                HostsDisponibles = db.Hosts.Select(h => new SelectListItem
                {
                    Value = h.HostId.ToString(),
                    Text = h.HostName
                }).ToList()
            };

            return View(model);
        }


        // POST: RadioPrograms/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizePermiso("EditRadioPrograms")]
        public ActionResult Edit(EditRadioProgramViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.HostsDisponibles = db.Hosts.Select(h => new SelectListItem
                {
                    Value = h.HostId.ToString(),
                    Text = h.HostName
                }).ToList();
                return View(model);
            }

            var programa = db.RadioPrograms
                            .Include("ProgramDays")
                            .Include("ProgramHosts")
                            .FirstOrDefault(p => p.ProgramId == model.ProgramId);

            if (programa == null)
            {
                return HttpNotFound();
            }

            // Actualizar propiedades básicas
            programa.ProgramName = model.ProgramName;
            programa.Image = model.Image;
            programa.Description = model.Description;
            programa.StartTime = model.StartTime;
            programa.EndTime = model.EndTime;

            db.ProgramDays.RemoveRange(programa.ProgramDays);
            foreach (var dia in model.SelectedDays)
            {
                db.ProgramDays.Add(new ProgramDays
                {
                    ProgramId = programa.ProgramId,
                    WeekDay = dia
                });
            }

            db.ProgramHosts.RemoveRange(programa.ProgramHosts);
            foreach (var hostId in model.SelectedHostIds)
            {
                db.ProgramHosts.Add(new ProgramHosts
                {
                    ProgramId = programa.ProgramId,
                    HostId = hostId
                });
            }

            db.Entry(programa).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("RadioProgramsTable");
        }


        // POST: RadioPrograms/Delete/5
        [AuthorizePermiso("DeleteRadioPrograms")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RadioPrograms radioPrograms = db.RadioPrograms.Find(id);

            var comments = db.CustomersComments
                 .Where(c => c.ProgramId == radioPrograms.ProgramId)
                 .ToList();

            foreach (var comment in comments)
            {
                db.CustomersComments.Remove(comment);
            }

            var programDays = db.ProgramDays
                .Where(pd => pd.ProgramId == radioPrograms.ProgramId)
                .ToList();
            foreach (var programDay in programDays)
                {
                db.ProgramDays.Remove(programDay);
            }
            var programHosts = db.ProgramHosts
                .Where(ph => ph.ProgramId == radioPrograms.ProgramId)
                .ToList();
            foreach (var programHost in programHosts)
                {
                db.ProgramHosts.Remove(programHost);
            }


            db.RadioPrograms.Remove(radioPrograms);
            db.SaveChanges();
            return RedirectToAction("RadioProgramsTable");
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
        [AuthorizePermiso("CreateComment")]
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
