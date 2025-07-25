﻿using ProyectVDEradio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using ProyectVDEradio.Utils;
using ProyectVDEradio.ViewModels;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;


namespace ProyectVDEradio.Controllers
{
    public class AccountController : Controller
    {
        private VozDelEsteDBEntities db = new VozDelEsteDBEntities();

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string hashedPassword = PasswordHelper.HashPassword(model.Password);

            var user = db.Users.FirstOrDefault(u =>
                (u.UserName == model.UsernameOrEmail || u.Email == model.UsernameOrEmail)
                && u.UserPassword == hashedPassword);

            if (user == null)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                return View(model);
            }

            var permisos = db.PermissionRole
                    .Where(rp => rp.RoleId == user.UserRole)
                    .Select(rp => rp.Permissions.PermissionName)
                    .ToList();

            // Crear claims
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.Roles.RoleName)
       
        };


            foreach (var permiso in permisos)
            {
                claims.Add(new Claim("Permiso", permiso));
            }


            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

            return RedirectToAction("Index", "Home");
        }


        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }


    }
}