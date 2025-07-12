using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectVDEradio.Models;
using ProyectVDEradio.Utils;
using ProyectVDEradio.ViewModels;

namespace ProyectVDEradio.Controllers
{
    public class UsersController : Controller
    {
        private VozDelEsteDBEntities db = new VozDelEsteDBEntities();

        // GET: Users
        // GET: Users
        [AuthorizePermiso("UsersTable")]
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Roles);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        [AuthorizePermiso("CreateUsers")]
        public ActionResult Create()
        {
            var model = new CreateUserViewModel
            {
                RolesDisponibles = db.Roles.Select(r => new SelectListItem
                {
                    Value = r.RoleId.ToString(),
                    Text = r.RoleName
                }).ToList()
            };
            return View(model);
        }

        // POST: Users/Create

        [AuthorizePermiso("CreateUsers")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hashedPass = PasswordHelper.HashPassword(model.UserPassword);

                var user = new Users
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    UserPassword = hashedPass,
                    UserRole = model.UserRole
                };

                db.Users.Add(user);
                db.SaveChanges();

                if (model.UserRole == 3)
                {
                    var cliente = new Customers
                    {
                        UserId = user.UserId,
                        CustomerName = model.Nombre,
                        CustomerSurname = model.Apellido,
                        BirthDate = model.FechaNacimiento?.Date ?? DateTime.Now
                    };
                    db.Customers.Add(cliente);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            model.RolesDisponibles = db.Roles.Select(r => new SelectListItem
            {
                Value = r.RoleId.ToString(),
                Text = r.RoleName
            });

            return View(model);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserRole = new SelectList(db.Roles, "RoleId", "RoleName", users.UserRole);
            return View(users);
        }

        // POST: Users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,Email,UserPassword,UserRole")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserRole = new SelectList(db.Roles, "RoleId", "RoleName", users.UserRole);
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
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
