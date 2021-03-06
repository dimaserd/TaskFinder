﻿using System;
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
using TwoStuWeb.Controllers.Base;
using System.Threading.Tasks;

namespace TwoStuWeb.Controllers
{
    
    public class HomeController : BaseController
    {
        #region Fields
        

        TaskSolutionSearcher _searcher;
        #endregion

        #region Properties
        
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
            //если дата подошла к концу разлогиниваем пользователя
            if(IsDateExpired())
            {
                return RedirectToAction("Quit", "Account");
            }

            ViewBag.Title = "Home Page";

            return View();
        }

        [Authorize]
        public ActionResult SearchTask()
        {
            //если дата подошла к концу разлогиниваем пользователя
            if (IsDateExpired())
            {
                return RedirectToAction("Quit", "Account");
            }

            
            return View();
            
            
        }

        [Authorize]
        public ActionResult SearchTaskByDescPartial(string desc = "")
        {
            using (SearchWorker searcher = new SearchWorker(new MyDbContext()))
            {

                var result = searcher.SearchByTaskDesc(desc: desc, searchFile: false, markText: true);
                return PartialView(result);
            }
        }

        [HttpGet]
        public async Task<ActionResult> SearchAnyTask(int? subjectId = null, int? workTypeId = null, int? subjectSectionId = null, bool? needSearch = false)
        {
            //если дата подошла к концу разлогиниваем пользователя
            if (IsDateExpired())
            {
                return RedirectToAction("Quit", "Account");
            }


            ViewBag.SubjectIdParam = subjectId;
            ViewBag.WorkTypeIdParam = workTypeId;
            ViewBag.SubjectSectionIdParam = subjectSectionId;
            ViewBag.NeedSearchParam = needSearch;

            ViewBag.WorkTypeId = new SelectList(await Db.WorkTypes.ToListAsync(), "Id", "Name");

            ViewBag.SubjectId = new SelectList(await Db.Subjects.ToListAsync(), "Id", "Name");

            return View();
        }


        [HttpGet]
        public PartialViewResult TasksSearch2(SearchSolutionsModel model)
        {
            List<TaskSolution> solutions = Searcher.SearchTasks(model).ToList();

            ViewBag.SearchModel = model;

            return PartialView(viewName:"TasksSearch", model: solutions);
        }

        [HttpPost]
        public PartialViewResult TasksSearch(SearchSolutionsModel model)
        {
            List<TaskSolution> solutions = Searcher.SearchTasks(model).ToList();

            ViewBag.SearchModel = model;

            return PartialView(solutions);
        }

        [Authorize]
        public string AuthCheck()
        {
            return "Auth";
        }
    }
}
