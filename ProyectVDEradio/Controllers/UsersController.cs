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
        [AuthorizePermiso("UsersTable")]
        public ActionResult Index()
        {
            var users = db.Users
                .Include(u => u.Roles)
                .Include(u => u.Customers) // ← incluye datos de cliente si existen
                .ToList();

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

            return View(viewModel);
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
        [AuthorizePermiso("UpdateUser")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            var viewModel = new CreateUserViewModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                // Podés dejar UserPassword vacío si no vas a editarlo
                UserRole = user.UserRole,
                RolesDisponibles = new SelectList(db.Roles, "RoleId", "RoleName", user.UserRole)
            };

            if (viewModel.UserRole == 3)
            {
                var cliente = new Customers
                {
                    UserId = viewModel.UserId,
                    CustomerName = viewModel.Nombre,
                    CustomerSurname = viewModel.Apellido,
                    BirthDate = viewModel.FechaNacimiento?.Date ?? DateTime.Now
                };
            }

            return View(viewModel);
        }


        // POST: Users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizePermiso("UpdateUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(model.UserId);
                if (user == null)
                    return HttpNotFound();

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.UserRole = model.UserRole;

                if (!string.IsNullOrWhiteSpace(model.UserPassword))
                {
                    user.UserPassword = PasswordHelper.HashPassword(model.UserPassword);
                }

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            model.RolesDisponibles = new SelectList(db.Roles, "RoleId", "RoleName", model.UserRole);
            return View(model);
        }


        // GET: Users/Delete/5
        [AuthorizePermiso("DeleteUser")]
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
        [AuthorizePermiso("DeleteUser")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Buscar y eliminar comentarios primero
            var customer = db.Customers.FirstOrDefault(c => c.UserId == id);
            if (customer != null)
            {
                var comments = db.CustomersComments
                                 .Where(c => c.CustomerId == customer.CustomerId)
                                 .ToList();

                foreach (var comment in comments)
                {
                    db.CustomersComments.Remove(comment);
                }

                db.Customers.Remove(customer); // Ahora sí se puede eliminar el cliente
            }

            // Luego eliminar el usuario
            Users users = db.Users.Find(id);
            if (users != null)
            {
                db.Users.Remove(users);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
