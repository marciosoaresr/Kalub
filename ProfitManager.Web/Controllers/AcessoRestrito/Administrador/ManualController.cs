using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using NHibernate.Mapping;
using PagedList;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.RepositorioWeb;

namespace ProfitManager.Web.Controllers.AcessoRestrito.Administrador
{
    public class ManualController : Controller
    {
        public ActionResult Index()
        {
            var manual = FuncoesManual.Load(1);

            return View(manual);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditarManual(Manual manual, FormCollection form)
        {
            ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                FuncoesManual.Salvar(manual);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", "Index");
            }
        }


        public bool VerificaMobile()
        {
            if (this.Request.Browser.IsMobileDevice || Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null ||
              (Request.ServerVariables["HTTP_ACCEPT"] != null && Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap")))
            {
                return true;
            }
            else if (Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                string[] mobiles = new[]
                {
                    "midp", "j2me", "avant", "docomo",
                    "novarra", "palmos", "palmsource",
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/",
                    "blackberry", "mib/", "symbian",
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio",
                    "SIE-", "SEC-", "samsung", "HTC",
                    "mot-", "mitsu", "sagem", "sony"
                    , "alcatel", "lg", "eric", "vx",
                    "NEC", "philips", "mmm", "xx",
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java",
                    "pt", "pg", "vox", "amoi",
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo",
                    "sgh", "gradi", "jb", "dddi","android",
                    "moto", "iphone", "ipad", "macintosh", "windows phone","linux"
                };

                foreach (string s in mobiles)
                {
                    if (Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower()))
                    {
                        return true;
                    }
                }

            }

            return false;
        }

    }
}
