using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProyectVDEradio.Models;
using ProyectVDEradio.ViewModels;

namespace ProyectVDEradio.Controllers
{
    public class RadioProgramsAPIController : ApiController
    {
        private VozDelEsteDBEntities db = new VozDelEsteDBEntities();

        // GET: api/RadioProgramsAPI
        [HttpGet]
        public IHttpActionResult GetRadioPrograms()
        {
            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;
            var currentDay = (int)now.DayOfWeek;

            var programaActual = db.RadioPrograms
                .ToList() // Lazy Loading necesita contexto activo y objetos vivos
                .Where(p => p.ProgramDays.Any(d => d.WeekDay == currentDay))
                .FirstOrDefault(p =>
                    (p.StartTime < p.EndTime && currentTime >= p.StartTime && currentTime < p.EndTime) || // mismo día
                    (p.StartTime > p.EndTime && (currentTime >= p.StartTime || currentTime < p.EndTime))   // cruza medianoche
                );

            int? idProgramaActual = programaActual?.ProgramId;

            var todosMenosElActual = db.RadioPrograms
                .Where(p => idProgramaActual == null || p.ProgramId != idProgramaActual)
                .OrderByDescending(p => p.StartTime)
                .ToList();

            // Lazy loading: al acceder a las propiedades de navegación, EF las carga si el contexto sigue abierto
            var response = new
            {
                RadioActual = programaActual == null ? null : new
                {
                    programaActual.ProgramId,
                    programaActual.ProgramName,
                    programaActual.Description,
                    programaActual.StartTime,
                    programaActual.EndTime,
                    Conductores = programaActual.ProgramHosts.Select(ph => new
                    {
                        ph.Hosts.HostId,
                        ph.Hosts.HostName
                    }),
                    Dias = programaActual.ProgramDays.Select(d => d.WeekDay)
                },

                OtrosProgramas = todosMenosElActual.Select(p => new
                {
                    p.ProgramId,
                    p.ProgramName,
                    p.Description,
                    p.StartTime,
                    p.EndTime
                }).ToList()
            };

            return Ok(response);
        }


        // GET: api/RadioProgramsAPI/5
        [ResponseType(typeof(RadioPrograms))]
        public IHttpActionResult GetRadioPrograms(int id)
        {
            RadioPrograms radioPrograms = db.RadioPrograms.Find(id);
            if (radioPrograms == null)
            {
                return NotFound();
            }

            return Ok(radioPrograms);
        }

        // PUT: api/RadioProgramsAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRadioPrograms(int id, RadioPrograms radioPrograms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != radioPrograms.ProgramId)
            {
                return BadRequest();
            }

            db.Entry(radioPrograms).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RadioProgramsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/RadioProgramsAPI
        [ResponseType(typeof(RadioPrograms))]
        public IHttpActionResult PostRadioPrograms(RadioPrograms radioPrograms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RadioPrograms.Add(radioPrograms);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = radioPrograms.ProgramId }, radioPrograms);
        }

        // DELETE: api/RadioProgramsAPI/5
        [ResponseType(typeof(RadioPrograms))]
        public IHttpActionResult DeleteRadioPrograms(int id)
        {
            RadioPrograms radioPrograms = db.RadioPrograms.Find(id);
            if (radioPrograms == null)
            {
                return NotFound();
            }

            db.RadioPrograms.Remove(radioPrograms);
            db.SaveChanges();

            return Ok(radioPrograms);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RadioProgramsExists(int id)
        {
            return db.RadioPrograms.Count(e => e.ProgramId == id) > 0;
        }
    }
}