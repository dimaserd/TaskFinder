using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models.TaskSolutions;
using TwoStu.Logic.Workers;
using System.Data.Entity;
using TwoStu.Logic.Workers.TaskSolutions;

namespace TwoStuWeb.Controllers
{
    
    public class HomeController : Controller
    {
        #region Fields
        static MyDbContext _db;

        TaskSolutionSearcher _searcher;
        #endregion

        #region Properties
        static MyDbContext Db
        {
            get
            {
                if (_db == null)
                {
                    _db = new MyDbContext();
                }
                return _db;
            }
        }

        TaskSolutionSearcher Searcher
        {
            get
            {
                if(_searcher == null)
                {
                    _searcher = new TaskSolutionSearcher(Db);
                }
                return _searcher;
            }
        }
        #endregion


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

        [HttpGet]
        public ActionResult SearchAnyTask()
        {
            ViewBag.WorkTypeId = new SelectList(Db.WorkTypes, "Id", "Name");

            ViewBag.SubjectId = new SelectList(Db.Subjects, "Id", "Name");

            return View();
        }


        [HttpPost]
        public PartialViewResult TasksSearch(SearchSolutionsModel model)
        {
            List<TaskSolution> solutions = Searcher.SearchTasks(model).ToList();

            return PartialView(solutions);
        }

        [Authorize]
        public string AuthCheck()
        {
            return "Auth";
        }
    }
}
