﻿using System.Web;
using System.Web.Optimization;

namespace ProyectVDEradio
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios.  De esta manera estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new Bundle("~/bundles/main").Include(
            "~/Scripts/main.js"));

            bundles.Add(new Bundle("~/bundles/Management").Include(
                      "~/Scripts/PagesScripts/Management.js"));

            bundles.Add(new Bundle("~/bundles/clima").Include(
            "~/Scripts/clima.js"));

            bundles.Add(new Bundle("~/bundles/dropdown").Include(
            "~/Scripts/DropDown.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Site.css",
                      "~/Content/Clima.css",
                      "~/Content/PagesStyles/Management.css",
                      "~/Content/UsersCRUDStyles/UsersCreate.css",
                      "~/Content/UsersCRUDStyles/UsersViewsStyles.css",
                      "~/Content/UsersCRUDStyles/UsersDelete.css"));
        }
    }
}
