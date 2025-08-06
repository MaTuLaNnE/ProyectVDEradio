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
    public class UsersAPIController : ApiController
    {
        private VozDelEsteDBEntities db = new VozDelEsteDBEntities();


        // GET: api/UsersAPI
        public IQueryable<UsersViewModel> GetUsers()
        {

            var users = db.Users
           .Include(u => u.Roles)
           .Include(u => u.Customers); // ← incluye datos de cliente si existen
          

            var viewModel = users.Select(u => new UsersViewModel
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                UserRole = u.UserRole,
                RoleName = u.Roles.RoleName,
                CustomerName = u.Customers.FirstOrDefault() != null ? u.Customers.FirstOrDefault().CustomerName : null,
                CustomerSurname = u.Customers.FirstOrDefault() != null ? u.Customers.FirstOrDefault().CustomerSurname : null,
                BirthDate = u.Customers.FirstOrDefault() != null ? u.Customers.FirstOrDefault().BirthDate : (DateTime?)null

            });

            return viewModel;
        }

        // GET: api/UsersAPI/5
        [ResponseType(typeof(Users))]
        public IHttpActionResult GetUsers(int id)
        {
            Users users = db.Users.Include(u => u.Roles)
                .Include(u => u.Customers).FirstOrDefault(u => u.UserId == id);


            if (users == null)
            {
                return NotFound();
            }

            var userViewModel = new UsersViewModel
            {
                UserId = users.UserId,
                UserName = users.UserName,
                Email = users.Email,
                UserRole = users.UserRole,
                RoleName = users.Roles.RoleName,
                CustomerName = users.Customers.FirstOrDefault() != null ? users.Customers.FirstOrDefault().CustomerName : null,
                CustomerSurname = users.Customers.FirstOrDefault() != null ? users.Customers.FirstOrDefault().CustomerSurname : null,
                BirthDate = users.Customers.FirstOrDefault() != null ? users.Customers.FirstOrDefault().BirthDate : (DateTime?)null
            };

            return Ok(userViewModel);
        }

        // PUT: api/UsersAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsers(int id, Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != users.UserId)
            {
                return BadRequest();
            }

            db.Entry(users).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

        // POST: api/UsersAPI
        [ResponseType(typeof(Users))]
        public IHttpActionResult PostUsers(Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(users);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = users.UserId }, users);
        }

        // DELETE: api/UsersAPI/5
        [ResponseType(typeof(Users))]
        public IHttpActionResult DeleteUsers(int id)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return NotFound();
            }

            db.Users.Remove(users);
            db.SaveChanges();

            return Ok(users);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsersExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}