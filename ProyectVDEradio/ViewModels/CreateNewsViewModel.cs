using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectVDEradio.ViewModels
{
    public class CreateNewsViewModel
    {
        [Display(Name = "Titulo De La Noticia")]
        public string Title { get; set; }

        [AllowHtml]
        [Display(Name = "Contenido De La Noticia")]
        public string Content { get; set; }

        [Display(Name = "Categoría")]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> CategoriasDisponibles { get; set; }

        [Display(Name = "Subtítulo")]
        public string ArticleSubtitle { get; set; }

        [Display(Name = "Imagen (URL o nombre de archivo)")]
        public string ArticleImage { get; set; }

        [Display(Name = "Fecha del Artículo")]
        [DataType(DataType.Date)]
        public DateTime ArticleDate { get; set; }
    }
}
