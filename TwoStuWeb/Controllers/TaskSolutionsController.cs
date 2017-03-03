using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models;
using TwoStu.Logic.Workers;
using System.Linq;
using TwoStu.Logic.Models.WorkerResults;
using System.Collections.Generic;
using TwoStu.Logic;
using TwoStu.Logic.Models.TaskSolutions;
using System;

namespace TwoStuWeb.Controllers
{
    [Authorize]
    public class TaskSolutionsController : Controller
    {
        #region Fields
        static MyDbContext _db;

        TaskSolutionsWorker _worker;
        #endregion

        #region Properties
        static MyDbContext Db
        {
            get
            {
                if(_db == null)
                {
                    _db = new MyDbContext();
                }
                return _db;
            }
        }

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
        public async Task<ActionResult> Index()
        {
            List<TaskSolution> model = await Worker.Db.TaskSolutions
                .Include(x => x.TaskSubject)
                .Include(x => x.TaskSubjectSection)
                .Include(x => x.SubjectDivisionChilds.Select(y => y.SubjectDivisionParent))
                .ToListAsync();
            return View(model.OrderByDescending(x => x.CreationDate));
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
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.WorkTypeId = new SelectList(Db.WorkTypes, "Id", "Name");

            ViewBag.SubjectId = new SelectList(Db.Subjects, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateSolutionModel model)
        {
            if(ModelState.IsValid)
            {
                WorkerResult result = await Worker.CreateSolution(model);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.WorkTypeId = new SelectList(Db.WorkTypes, "Id", "Name");

            ViewBag.SubjectId = new SelectList(Db.Subjects, "Id", "Name");

            return View();
        }

        // GET: TaskSolutions/Create
        public ActionResult CreatePhysics()
        {
            int physicsId = Db.Subjects.FirstOrDefault(x => x.Name == "Физика").Id;

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
            

            int physicsId = Db.Subjects.FirstOrDefault(x => x.Name == "Физика").Id;

            ViewBag.SubjectSectionId = new SelectList(Db.SubjectSections.Where(x => x.SubjectId == physicsId), "Id", "Name");
            ViewBag.WorkTypeId = new SelectList(Db.WorkTypes, "Id", "Name");

            

            if (ModelState.IsValid)
            {
                WorkerResult result = await Worker.CreatePhysicsSolution(model);
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

        #region Edit methods
        // GET: TaskSolutions/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
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

        #region Help Methods
        WorkerResult UserHasRightsToBeThere()
        {
            if(!User.IsInRole("Admin"))
            {
                return new WorkerResult("У вас недостаточно прав!");
            }

            return new WorkerResult
            {
                Succeeded = true
            };
        }


        void AddErrors(WorkerResult workerResult)
        {
            foreach(string error in workerResult.ErrorsList)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IDisposable[] toDisposes = new IDisposable[]
                {
                    //_worker, _db
                };
                for(int i = 0; i < toDisposes.Length; i++)
                {
                    if(toDisposes[i] != null)
                    {
                        toDisposes[i].Dispose();
                        toDisposes[i] = null;
                    }
                }
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
