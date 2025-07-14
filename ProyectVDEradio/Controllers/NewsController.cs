using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProyectVDEradio.Models;
using ProyectVDEradio.Utils.WeatherAPI;
using ProyectVDEradio.ViewModels;
using ProyectVDEradio.Utils.WeatherForecastAPI;
using ProyectVDEradio.Utils;
using Newtonsoft.Json;
using ProyectVDEradio.DataAccess.APIsRepository;
using System.Security.Claims;


namespace ProyectVDEradio.Controllers
{
    public class NewsController : Controller
    {
        private VozDelEsteDBEntities db = new VozDelEsteDBEntities();

        APIs api = new APIs();

        public ActionResult IndexPolicial()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Policial")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            var model = new WeatherViewModel
            {
                noticias = noticias,
                Weather = new WeatherData()
            };

            return View("Categoria", model);
        }

        public async Task<ActionResult> IndexEconomia()
        {
            var modelo = new WeatherViewModel();
            modelo.noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Economía")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            //var ultima = db.CurrencyAudits.Where(c => c.CurrencyAuditDate >  DateTime.Today).ToList();

            //var existingRates = api.AddCurrencyAudit();

            var service = new CurrencyService();
            var existingRates = service.GetLatestAuditIfRecent();

            decimal usd, ars, brl;

            if (existingRates == null)
            {
                // No hay datos en las ultimas 24hrs, llamar a la API
                string urlCurrency = "http://apilayer.net/api/live?access_key=3b51394b74c5ace5216edbb5731c5961&currencies=BRL,USD,ARS&source=UYU&format=1";

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(urlCurrency);
                    var currency = Utils.CurrencyApi.FromJson(response);

                    usd = 1/currency.Quotes.Uyuusd ?? 0;
                    ars = 1/currency.Quotes.Uyuars ?? 0;
                    brl = 1/currency.Quotes.Uyubrl ?? 0;

                    // Insertar en CurrencyAudit
                    await service.InsertCurrencyAuditAsync(usd, ars, brl);
                    //api.AddCurrencyAudit(usd, ars, brl); // ARREGLAR ESTO Y DESDE LA API
                } 
            }
            else
            {
                // Si hay cotizaciones recientes, usarlas
                usd = existingRates.Value.usd;
                ars = existingRates.Value.ars;
                brl = existingRates.Value.brl;
            }

            modelo.Currency = new Currency
            {
                UYUUSD = usd,
                UYUARS = ars,
                UYUBRL = brl,
                TimeStamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()
            };

            return View("Categoria", modelo);
        }

        public JsonResult GetCurrencyHistory()
        {
            try
            {
                var service = new CurrencyService();
                var historial = service.GetCurrencyHistory();

                var data = new
                {
                    labels = historial.Select(h => h.Timestamp.ToString("dd/MM HH:mm")).ToArray(),
                    datasets = new[]
                    {
                new {
                    label = "USD",
                    data = historial.Select(h => h.UYUUSD).ToArray(),
                    borderColor = "#28a745",
                    backgroundColor = "rgba(40, 167, 69, 0.1)",
                    tension = 0.1
                },
                new {
                    label = "ARS",
                    data = historial.Select(h => h.UYUARS).ToArray(),
                    borderColor = "#007bff",
                    backgroundColor = "rgba(0, 123, 255, 0.1)",
                    tension = 0.1
                },
                new {
                    label = "BRL",
                    data = historial.Select(h => h.UYUBRL).ToArray(),
                    borderColor = "#ffc107",
                    backgroundColor = "rgba(255, 193, 7, 0.1)",
                    tension = 0.1
                }
            }
                };

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult IndexDeportes()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Deportes")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            var model = new WeatherViewModel
            {
                noticias = noticias,
                Weather = new WeatherData()
            };

            return View("Categoria", model);
        }

        public ActionResult IndexTurismo()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Turismo")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            var model = new WeatherViewModel
            {
                noticias = noticias,
                Weather = new WeatherData()
            };

            return View("Categoria", model);
        }

        public ActionResult IndexEmpresarial()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Empresarial")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            var model = new WeatherViewModel
            {
                noticias = noticias,
                Weather = new WeatherData()
            };

            return View("Categoria", model);
        }

        public ActionResult IndexPolitica()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Política")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            var model = new WeatherViewModel
            {
                noticias = noticias,
                Weather = new WeatherData()
            };

            return View("Categoria", model);
        }

        public async Task<ActionResult> IndexClima()
        {
            var modelo = new WeatherViewModel();


            modelo.noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Clima")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();


            // --- Clima actual ---
            string urlClima = "https://api.openweathermap.org/data/2.5/weather?lat=-34.90&lon=-54.95&appid=7d3b86d0d678a4b70d2a0d0d027d78f1&units=metric&lang=es";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(urlClima);
                var clima = Utils.WeatherAPI.Welcome.FromJson(response);

                modelo.Weather = new WeatherData
                {
                    Temp = clima.Main.Temp ?? 0,
                    Estado = clima.Weather[0].Description,
                    Icono = clima.Weather[0].Icon,
                    TempMax = clima.Main.TempMax ?? 0,
                    TempMin = clima.Main.TempMin ?? 0,
                    Humedad = (int)(clima.Main.Humidity ?? 0),
                    Viento = clima.Wind.Speed ?? 0,
                    Presion = (int)(clima.Main.Pressure ?? 0),
                    Sensacion = clima.Main.FeelsLike ?? 0,
                    Amanecer = clima.Sys.Sunrise ?? 0,
                    Atardecer = clima.Sys.Sunset ?? 0
                };

                // --- Pronóstico 5 días ---
                string urlForecast = "https://api.openweathermap.org/data/2.5/forecast?lat=-34.90&lon=-54.95&appid=7d3b86d0d678a4b70d2a0d0d027d78f1&units=metric&lang=es";
                var forecastResponse = await client.GetStringAsync(urlForecast);
                var pronostico = Utils.WeatherForecastAPI.WeatherForecastApi.FromJson(forecastResponse);

                modelo.Forecast = pronostico.List
                                .GroupBy(f => f.DtTxt.Value.Date)
                                .Select(grupo =>
                                 {
                                     var muestraRepresentativa = grupo.FirstOrDefault(x => x.DtTxt.Value.Hour == 12) ?? grupo.First();

                                     return new ForecastData
                                     {
                                         Fecha = grupo.Key,
                                         Temp = muestraRepresentativa.Main.Temp ?? 0,
                                         Estado = muestraRepresentativa.Weather.FirstOrDefault()?.Description ?? "",
                                         Icono = muestraRepresentativa.Weather.FirstOrDefault()?.Icon ?? "",
                                         TempMax = grupo.Max(x => x.Main.TempMax ?? 0),
                                         TempMin = grupo.Min(x => x.Main.TempMin ?? 0),
                                         Humedad = (int)(muestraRepresentativa.Main.Humidity ?? 0),
                                         Viento = muestraRepresentativa.Wind.Speed ?? 0,
                                         Presion = (int)(muestraRepresentativa.Main.Pressure ?? 0),
                                         Sensacion = muestraRepresentativa.Main.FeelsLike ?? 0
                                     };
                                 })
                                .ToList();

            }

            return View("Categoria", modelo);
        }



        public ActionResult IndexTransporte()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Transporte")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            var model = new WeatherViewModel
            {
                noticias = noticias,
                Weather = new WeatherData()
            };

            return View("Categoria", model);
        }

        public ActionResult IndexInternacional()
        {
            var noticias = db.News
                .Include(n => n.Categories)
                .Where(n => n.Categories.CategoryName == "Internacional")
                .OrderByDescending(n => n.ArticleDate)
                .ToList();

            var model = new WeatherViewModel
            {
                noticias = noticias,
                Weather = new WeatherData()
            };

            return View("Categoria", model);
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
            var model = new CreateNewsViewModel
            {
                CategoriasDisponibles = db.Categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList()
            };

            return View(model);
        }

        // POST: News/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateNewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
                int userId = int.Parse(userIdClaim.Value);

                var noticia = new News
                {
                    ArticleTitle = model.Title,
                    ArticleContent = model.Content,
                    CategoryId = model.CategoryId,
                    ArticleSubtitle = model.ArticleSubtitle,
                    ArticleImage = model.ArticleImage,
                    ArticleDate = DateTime.Now,
                    AuthorId = userId,
                };

                db.News.Add(noticia);
                db.SaveChanges();
                return RedirectToAction("index", "Management");
            }

            // Volvemos a cargar las categorías si hay error
            model.CategoriasDisponibles = db.Categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();

            return View(model);
        }

        public ActionResult NewsTable()
        {
            var noticias = db.News.Include("Categories").Include("Users").ToList();
            return View(noticias);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var noticia = db.News.Find(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }

            var model = new EditNewsViewModel
            {
                ArticleID = noticia.ArticleID,
                ArticleTitle = noticia.ArticleTitle,
                ArticleContent = noticia.ArticleContent,
                CategoryId = noticia.CategoryId,
                ArticleDate = noticia.ArticleDate,
                ArticleImage = noticia.ArticleImage,
                ArticleSubtitle = noticia.ArticleSubtitle,
                AuthorId = noticia.AuthorId,
                CategoriasDisponibles = db.Categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName,
                    Selected = c.CategoryId == noticia.CategoryId
                }).ToList()
            };

            return View(model);
        }

        // POST: News/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditNewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var noticia = db.News.Find(model.ArticleID);
                if (noticia == null)
                {
                    return HttpNotFound();
                }

                noticia.ArticleTitle = model.ArticleTitle;
                noticia.ArticleContent = model.ArticleContent;
                noticia.CategoryId = model.CategoryId;
                noticia.ArticleDate = model.ArticleDate;
                noticia.ArticleImage = model.ArticleImage;
                noticia.AuthorId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier) != null ? 
                    int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value) : 0;  
                noticia.ArticleSubtitle = model.ArticleSubtitle;

                db.Entry(noticia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NewsTable");
            }

            model.CategoriasDisponibles = db.Categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            });

            return View(model);
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
