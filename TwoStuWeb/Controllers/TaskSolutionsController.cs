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

namespace TwoStuWeb.Controllers
{
    [Authorize]
    public class TaskSolutionsController : Controller
    {
        #region Fields
        private TaskSolutionsWorker worker = new TaskSolutionsWorker();
        #endregion

        #region HttpController methods
        // GET: TaskSolutions
        public async Task<ActionResult> Index()
        {
            List<TaskSolution> model = await worker.db.TaskSolutions.ToListAsync();
            return View(model.OrderByDescending(x => x.CreationDate));
        }

        // GET: TaskSolutions/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskSolution taskSolution = await worker.db.TaskSolutions
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

        // GET: TaskSolutions/Create
        public ActionResult Create()
        {
            int physicsId = worker.db.Subjects.FirstOrDefault(x => x.Name == "Физика").Id;

            ViewBag.SubjectSectionId = new SelectList(worker.db.SubjectSections.Where(x => x.SubjectId == physicsId), "Id", "Name");
            ViewBag.WorkTypeId = new SelectList(worker.db.WorkTypes, "Id", "Name");
            return View();
        }

        // POST: TaskSolutions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreatePhysicsSolutionModel model)
        {
            

            int physicsId = worker.db.Subjects.FirstOrDefault(x => x.Name == "Физика").Id;

            ViewBag.SubjectSectionId = new SelectList(worker.db.SubjectSections.Where(x => x.SubjectId == physicsId), "Id", "Name");
            ViewBag.WorkTypeId = new SelectList(worker.db.WorkTypes, "Id", "Name");

            

            if (ModelState.IsValid)
            {
                WorkerResult result = await worker.CreatePhysicsSolution(model);
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
            TaskSolution taskSolution = await worker.db.TaskSolutions.FindAsync(id);
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
            if (ModelState.IsValid)
            {
                worker.db.Entry(taskSolution).State = EntityState.Modified;
                await worker.db.SaveChangesAsync();
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
            TaskSolution taskSolution = await worker.db.TaskSolutions.FindAsync(id);
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
            TaskSolution taskSolution = await worker.db.TaskSolutions.FindAsync(id);
            worker.db.TaskSolutions.Remove(taskSolution);
            await worker.db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #endregion

        #region Help Methods
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
                worker.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
