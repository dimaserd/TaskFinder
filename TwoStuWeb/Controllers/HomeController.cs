using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Workers;

namespace TwoStuWeb.Controllers
{
    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [Authorize]
        public ActionResult SearchTask(string desc = "")
        {
            using (SearchWorker searcher = new SearchWorker(new MyDbContext()))
            {
                
                var result = searcher.SearchByTaskDesc(desc: desc, searchFile: false, markText: true);
                return View(result);
            }
            
        }

        [Authorize]
        public string AuthCheck()
        {
            return "Auth";
        }
    }
}
