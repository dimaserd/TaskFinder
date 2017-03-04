using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models.WorkerResults;
using System.Collections.Generic;
using TwoStu.Logic.Models;

namespace TwoStuWeb.Controllers
{
    [Authorize]
    public class SubjectSectionsController : Controller
    {
        #region Fields
        private MyDbContext db = new MyDbContext();
        #endregion

        #region HttpController methods
        // GET: SubjectSections
        public async Task<ActionResult> Index()
        {
            List<Subject> subjects = await db.Subjects
                .Include(x => x.SubjectSections.Select(y => y.FromSubject))
                .ToListAsync();

            List<Subject> userSubjects = User.Identity.GetUserSubjects(subjects).ToList();



            
            return View(userSubjects);
        }

        // GET: SubjectSections/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectSection subjectSection = await db.SubjectSections
                .Include(x => x.SubjectDivisions.Select(k => k.SubjectDivisionChilds))
                .Include(x => x.FromSubject)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (subjectSection == null)
            {
                return HttpNotFound();
            }
            return View(subjectSection);
        }

        #region Create methods
        // GET: SubjectSections/Create
        public ActionResult Create(int? toSubjectId = null)
        {
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name");
            ViewBag.ToSubjectId = toSubjectId;

            return View(new SubjectSection { Id = 2});
        }

        // POST: SubjectSections/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,SubjectId")] SubjectSection subjectSection)
        {
            WorkerResult result = await UserHasRightsToCreateThatSubjectSectionAsync(subjectSection);
            if (!result.Succeeded)
            {
                AddErrors(result);
            }

            if (ModelState.IsValid)
            {

                subjectSection.Id = db.SubjectSections.Count() + 1;
                db.SubjectSections.Add(subjectSection);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name", subjectSection.SubjectId);
            return View(subjectSection);
        }

        #endregion

        #region Edit methods
        // GET: SubjectSections/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WorkerResult result = await UserHasRightsForThatSubjectSectionAsync(id.Value);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            SubjectSection subjectSection = await db.SubjectSections.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (subjectSection == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name", subjectSection.SubjectId);
            return View(subjectSection);
        }

        // POST: SubjectSections/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,SubjectId")] SubjectSection subjectSection)
        {

            WorkerResult result = await UserHasRightsForThatSubjectSectionAsync(subjectSection.Id);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                db.Entry(subjectSection).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name", subjectSection.SubjectId);
            return View(subjectSection);
        }

        #endregion

        #region Delete methods
        // GET: SubjectSections/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            WorkerResult result = UserHasRightsToBeThere();
            if(!result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectSection subjectSection = await db.SubjectSections
                //.Include(x => x.TaskSolutions)
                .Include(x => x.SubjectDivisions.Select(y => y.SubjectDivisionChilds.Select(z => z.TaskSolutions)))
                .FirstOrDefaultAsync(x => x.Id == id);

            if (subjectSection == null)
            {
                return HttpNotFound();
            }
            return View(subjectSection);
        }

        // POST: SubjectSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WorkerResult result = UserHasRightsToBeThere();
            if (!result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            SubjectSection subjectSection = await db.SubjectSections.FindAsync(id);
            db.SubjectSections.Remove(subjectSection);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #endregion

        #region Help methods
        WorkerResult UserHasRightsToBeThere()
        {
            if (!User.IsInRole("Admin"))
            {
                return new WorkerResult("У вас недостаточно прав!");
            }

            return new WorkerResult
            {
                Succeeded = true
            };
        }

        async Task<WorkerResult> UserHasRightsForThatSubjectSectionAsync(int subjectSectionId)
        {
            List<Subject> subjects = await db.Subjects.ToListAsync();

            //получили список предметов пользователя
            List<Subject> userSubjects = User.Identity.GetUserSubjects(subjects).ToList();

            SubjectSection subjectSection = await db.SubjectSections
                .Include(x => x.FromSubject)
                .FirstOrDefaultAsync(x => x.Id == subjectSectionId);

            if(subjectSection == null)
            {
                return new WorkerResult("Раздел предмета не найден!");
            }

            Subject subjectFromSection = subjectSection.FromSubject;

            if(userSubjects.Any(x => x.Id == subjectFromSection.Id))
            {
                return new WorkerResult
                {
                    Succeeded = true
                };
            }


            return new WorkerResult($"У вас недостаточно прав для данного раздела!");
        }

        async Task<WorkerResult> UserHasRightsToCreateThatSubjectSectionAsync(SubjectSection subjectSection)
        {
            List<Subject> subjects = await db.Subjects.ToListAsync();

            //получили список предметов пользователя
            List<Subject> userSubjects = User.Identity.GetUserSubjects(subjects).ToList();

            if(userSubjects.Any(x => x.Id == subjectSection.SubjectId))
            {
                return new WorkerResult
                {
                    Succeeded = true
                };

            }

            return new WorkerResult("У вас недостаточно прав для создания раздела по данному предмету!");
        }

        void AddErrors(WorkerResult workerResult)
        {
            foreach (string error in workerResult.ErrorsList)
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
