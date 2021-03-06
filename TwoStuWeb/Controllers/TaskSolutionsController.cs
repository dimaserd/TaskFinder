﻿using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models;
using TwoStu.Logic.Workers;
using System.Linq;
using TwoStu.Logic.Models.WorkerResults;
using System.Collections.Generic;
using System;
using TwoStuWeb.Controllers.Base;
using TwoStu.Logic.Models.TaskSolutions.Base;

namespace TwoStuWeb.Controllers
{
    [Authorize]
    public class TaskSolutionsController : BaseController
    {
        #region Поля
        

        TaskSolutionsWorker _worker;
        #endregion

        #region Свойства
        

        TaskSolutionsWorker Worker
        {
            get
            {
                if(_worker == null)
                {
                    _worker = new TaskSolutionsWorker(Db);
                }
                return _worker;
            }
        }
        #endregion

        #region HttpController methods
        // GET: TaskSolutions
        public async Task<ActionResult> Index(int page = 1)
        {
            DateTime startDate = DateTime.Now;

            List<TaskSolution> model = await Worker.Db.TaskSolutions
                .OrderByDescending(x => x.CreationDate)
                .Skip((page - 1) * 100)
                .Take(100)
                .Include(x => x.TaskSubject)
                .Include(x => x.TaskSubjectSection)
                .Include(x => x.Versions)
                .Include(x => x.SubjectDivisionChilds.Select(y => y.SubjectDivisionParent))
                .ToListAsync();

            DateTime finishDate = DateTime.Now;

            double? milliSecs = (finishDate - startDate).TotalMilliseconds;

            ViewData["milliSecs"] = milliSecs;
            ViewData["page"] = page;

            return View(model.OrderBy(x => x.Versions.Count));
        }

        // GET: TaskSolutions/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskSolution taskSolution = await Worker.Db.TaskSolutions
                .Include(x => x.TypeOfWork)
                .Include(x => x.TaskSubject)
                .Include(x => x.TaskSubjectSection)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (taskSolution == null)
            {
                return HttpNotFound();
            }
            return View(taskSolution);
        }

        #region Create methods

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.WorkTypeId = new SelectList(Db.WorkTypes, "Id", "Name");

            ViewBag.SubjectId = new SelectList(User.Identity.GetUserSubjects(Db.Subjects), "Id", "Name");

            return View();
        }

        /// <summary>
        /// Защиту можно пока обойти так как нет проверки на раздел предмета
        /// тем самым нужно проверять принадлежит ли указанный раздел предмета к 
        /// самому предмету и тоже самое с уточнениями
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(CreateSolutionModelBase model)
        {
            
            WorkerResult hasRights = await UserHasRightsForThatSubjectAsync(model.SubjectId);
            if(!hasRights.Succeeded)
            {
                AddErrors(hasRights);
            }

            if (ModelState.IsValid)
            {
                WorkerResult result = await Worker.CreateSolutionAsync(model, checkDesc: false);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                AddErrors(result);
            }

            ViewBag.WorkTypeId = new SelectList(Db.WorkTypes, "Id", "Name");

            ViewBag.SubjectId = new SelectList(Db.Subjects, "Id", "Name");

            
            return View(model);
        }

        #endregion

        #region CreatePhysics

        // GET: TaskSolutions/Create
        public async Task<ActionResult> CreatePhysics()
        {
            int physicsId = Db.Subjects.FirstOrDefault(x => x.Name == "Физика").Id;

            WorkerResult hasRights = await UserHasRightsForThatSubjectAsync(physicsId);
            if (!hasRights.Succeeded)
            {
                return RedirectToAction("Create");
            }
            

            ViewBag.SubjectSectionId = new SelectList(Db.SubjectSections.Where(x => x.SubjectId == physicsId), "Id", "Name");
            ViewBag.WorkTypeId = new SelectList(Db.WorkTypes, "Id", "Name");
            return View();
        }

        // POST: TaskSolutions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePhysics(CreatePhysicsSolutionModel model)
        {
            int physicsId = (await Db.Subjects.FirstOrDefaultAsync(x => x.Name == "Физика")).Id;

            WorkerResult hasRightsResult = await UserHasRightsForThatSubjectAsync(physicsId);
            if (!hasRightsResult.Succeeded)
            {
                AddErrors(hasRightsResult);
            }

            
            ViewBag.SubjectSectionId = new SelectList(Db.SubjectSections.Where(x => x.SubjectId == physicsId), "Id", "Name");
            ViewBag.WorkTypeId = new SelectList(Db.WorkTypes, "Id", "Name");

            


            if (ModelState.IsValid)
            {
                //WorkerResult result = await Worker.CreatePhysicsSolution(model);
                WorkerResult result = await Worker.CreatePhysicsSolutionAsync(model);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                    return View(model);
                }
                
            }

            return View(model);
        }

        #endregion

        #endregion

        #region Edit methods
        // GET: TaskSolutions/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            WorkerResult hasRights = UserHasRightsToBeThere();
            if(!hasRights.Succeeded)
            {
                return RedirectToAction("Index");
            }
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskSolution taskSolution = await Db.TaskSolutions.FindAsync(id);
            if (taskSolution == null)
            {
                return HttpNotFound();
            }

            
            return View(taskSolution);
        }

        // POST: TaskSolutions/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SubjectType,Subject,TaskDesc,FilePath,FileName")] TaskSolution taskSolution)
        {
            WorkerResult hasRights = UserHasRightsToBeThere();

            if (!hasRights.Succeeded)
            {
                AddErrors(hasRights);

                return View(taskSolution);
            }

            if (ModelState.IsValid)
            {
                Db.Entry(taskSolution).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(taskSolution);
        }

        #endregion

        #region Delete methods
        // GET: TaskSolutions/Delete/5
        public async Task<ActionResult> Delete(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WorkerResult hasRights = UserHasRightsToBeThere();
            if (!hasRights.Succeeded)
            {

                return RedirectToAction("Index");
            }

            TaskSolution taskSolution = await Db.TaskSolutions.FindAsync(id);
            if (taskSolution == null)
            {
                return HttpNotFound();
            }
            return View(taskSolution);
        }

        //ничего не удаляет
        // POST: TaskSolutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            WorkerResult hasRights = UserHasRightsToBeThere();
            if(!hasRights.Succeeded)
            {
                
                return RedirectToAction("Index");
            }

            WorkerResult result = await Worker.DeleteSolution(id);

            if(!result.Succeeded)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #endregion

        
    }
}
