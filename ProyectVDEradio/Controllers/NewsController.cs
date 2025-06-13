using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectVDEradio.Models;

namespace ProyectVDEradio.Controllers
{
    public class NewsController : Controller
    {
        private VozDelEsteDBEntities db = new VozDelEsteDBEntities();

        public ActionResult IndexPolicial()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Policial")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            return View("Categoria", noticias); 
        }

        public ActionResult IndexEconomia()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Economía")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            return View("Categoria", noticias); 
        }

        public ActionResult IndexDeportes()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Deportes")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            return View("Categoria", noticias); 
        }

        public ActionResult IndexTurismo()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Turismo")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            return View("Categoria", noticias); 
        }

        public ActionResult IndexEmpresarial()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Empresarial")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            return View("Categoria", noticias); 
        }

        public ActionResult IndexPolitica()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Política")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            return View("Categoria", noticias); 
        }

        public ActionResult IndexClima()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Clima")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            return View("Categoria", noticias);
        }

        public ActionResult IndexTransporte()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Transporte")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            return View("Categoria", noticias); 
        }

        public ActionResult IndexInternacional()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Internacional")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            return View("Categoria", noticias); 
        }

        public ActionResult Details(int id)
        {
            var noticia = db.News.Include(n => n.Categories).FirstOrDefault(n => n.ArticleID == id);
            if (noticia == null)
                return HttpNotFound();

            return View(noticia);
        }


        // GET: News/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.AuthorId = new SelectList(db.Users, "UserId", "UserName");
            return View();
        }

        // POST: News/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleID,ArticleTitle,ArticleContent,ArticleDate,ArticleImage,AuthorId,CategoryId,ArticleSubtitle")] News news)
        {
            if (ModelState.IsValid)
            {
                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", news.CategoryId);
            ViewBag.AuthorId = new SelectList(db.Users, "UserId", "UserName", news.AuthorId);
            return View(news);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", news.CategoryId);
            ViewBag.AuthorId = new SelectList(db.Users, "UserId", "UserName", news.AuthorId);
            return View(news);
        }

        // POST: News/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleID,ArticleTitle,ArticleContent,ArticleDate,ArticleImage,AuthorId,CategoryId,ArticleSubtitle")] News news)
        {
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", news.CategoryId);
            ViewBag.AuthorId = new SelectList(db.Users, "UserId", "UserName", news.AuthorId);
            return View(news);
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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
