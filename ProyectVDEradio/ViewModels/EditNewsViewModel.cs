using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectVDEradio.ViewModels
{
    public class EditNewsViewModel
    {
        public int ArticleID { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        public string ArticleTitle { get; set; }

        [Required(ErrorMessage = "El contenido es obligatorio")]
        [DataType(DataType.MultilineText)]
        public string ArticleContent { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        [Display(Name = "Categoría")]
        public int CategoryId { get; set; }

        [Display(Name = "Fecha del Artículo")]
        [DataType(DataType.Date)]
        public DateTime ArticleDate { get; set; }

        [Display(Name = "Imagen (URL o ruta)")]
        public string ArticleImage { get; set; }

        [Display(Name = "Subtítulo")]
        public string ArticleSubtitle { get; set; }
        public int AuthorId { get; set; }

        public IEnumerable<SelectListItem> CategoriasDisponibles { get; set; }
    }
}