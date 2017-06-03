using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models.WorkerResults;
using System.Collections.Generic;
using TwoStu.Logic.Models;
using TwoStuWeb.Controllers.Base;
using System.Linq;

namespace TwoStuWeb.Controllers
{
    [Authorize]
    public class DivisionChildsController : BaseController
    {
        #region Поля
        private MyDbContext db = new MyDbContext();
        #endregion

        #region HttpController methods
        // GET: DivisionChilds
        public async Task<ActionResult> Index()
        {
            var subjectDivisionChilds = (db.SubjectDivisionChilds.Include(s => s.SubjectDivisionParent.FromSubjectSection.FromSubject));

            return View(
                (await subjectDivisionChilds.ToListAsync())
                .OrderBy(x => x.SubjectDivisionParent.FromSubjectSection.FromSubject)
                .OrderBy(x => x.SubjectDivisionParent.FromSubjectSection)
                );
        }

        
        #region Details methods
        // GET: DivisionChilds/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectDivisionChild subjectDivisionChild = await db.SubjectDivisionChilds.FindAsync(id);
            if (subjectDivisionChild == null)
            {
                return HttpNotFound();
            }
            return View(subjectDivisionChild);
        }

        #endregion

        #region Details methods
        // GET: DivisionChilds/Create
        public ActionResult Create(int? toSubjectDivisionId = null)
        {
            ViewBag.SubjectDivisionId = new SelectList(db.SubjectDivisions, "Id", "Name");

            ViewBag.toSubjectDivisionId = toSubjectDivisionId;

            return View();
        }

        // POST: DivisionChilds/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,SubjectDivisionId")] SubjectDivisionChild subjectDivisionChild)
        {
            if (ModelState.IsValid)
            {
                db.SubjectDivisionChilds.Add(subjectDivisionChild);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectDivisionId = new SelectList(db.SubjectDivisions, "Id", "Name", subjectDivisionChild.SubjectDivisionId);
            return View(subjectDivisionChild);
        }
        #endregion

        #region Edit methods
        // GET: DivisionChilds/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WorkerResult hasRights = await UserHasRightsForThatSubjectDivisionChildAsync(id.Value);
            if (!hasRights.Succeeded)
            {
                return RedirectToAction("Index");
            }

            SubjectDivisionChild subjectDivisionChild = await db.SubjectDivisionChilds.FindAsync(id);
            if (subjectDivisionChild == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectDivisionId = new SelectList(db.SubjectDivisions, "Id", "Name", subjectDivisionChild.SubjectDivisionId);
            return View(subjectDivisionChild);
        }

        // POST: DivisionChilds/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,SubjectDivisionId")] SubjectDivisionChild subjectDivisionChild)
        {
            WorkerResult hasRights = await UserHasRightsForThatSubjectDivisionChildAsync(subjectDivisionChild.Id);
            if (!hasRights.Succeeded)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                db.Entry(subjectDivisionChild).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectDivisionId = new SelectList(db.SubjectDivisions, "Id", "Name", subjectDivisionChild.SubjectDivisionId);
            return View(subjectDivisionChild);
        }

        #endregion

        #region Delete methods
        // GET: DivisionChilds/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            WorkerResult hasRights = UserHasRightsToBeThere();
            if (!hasRights.Succeeded)
            {
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectDivisionChild subjectDivisionChild = await db.SubjectDivisionChilds.FindAsync(id);
            if (subjectDivisionChild == null)
            {
                return HttpNotFound();
            }
            return View(subjectDivisionChild);
        }

        // POST: DivisionChilds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WorkerResult hasRights = UserHasRightsToBeThere();
            if (!hasRights.Succeeded)
            {
                return RedirectToAction("Index");
            }

            SubjectDivisionChild subjectDivisionChild = await db.SubjectDivisionChilds.FindAsync(id);
            db.SubjectDivisionChilds.Remove(subjectDivisionChild);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #endregion

        #region Вспомогальные методы
        

        async Task<WorkerResult> UserHasRightsForThatSubjectDivisionChildAsync(int subjectDivisionChildId)
        {
            SubjectDivisionChild child = await db.SubjectDivisionChilds
                .Include(x => x.SubjectDivisionParent.FromSubjectSection.FromSubject)
                .FirstOrDefaultAsync(x => x.Id == subjectDivisionChildId);

            if(child == null)
            {
                return new WorkerResult("Такого варианта уточнения не существует");
            }

            Subject subjectFromChild = child.SubjectDivisionParent.FromSubjectSection.FromSubject;

            List<Subject> subjects = await db.Subjects.ToListAsync();

            if(User.Identity.HasUserThatSubjectFromList(subjects,subjectFromChild))
            {
                return new WorkerResult
                {
                    Succeeded = true
                };
            }

            return new WorkerResult("У вас нет прав для редактирования этого варианта уточнения");
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
    }
        #endregion
}
