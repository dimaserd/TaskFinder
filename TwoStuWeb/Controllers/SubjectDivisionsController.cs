using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models.WorkerResults;
using System.Linq;
using System.Collections.Generic;
using TwoStu.Logic.Models;
using TwoStuWeb.Controllers.Base;

namespace TwoStuWeb.Controllers
{
    [Authorize]
    public class SubjectDivisionsController : BaseController
    {
        private MyDbContext db = new MyDbContext();

        #region HttpController methods
        // GET: SubjectDivisions
        public async Task<ActionResult> Index()
        {
            //List<SubjectSection> subjectSections = await db.SubjectSections
            //    .Include(x => x.SubjectDivisions.Select(y => y.SubjectDivisionChilds))
            //    .ToListAsync();

            List<Subject> subjects = await db.Subjects
                .Include(x => x.SubjectSections.Select(y => y.SubjectDivisions.Select(z => z.SubjectDivisionChilds.Select(q => q.TaskSolutions))))
                .Include(x => x.SubjectSections.Select(y => y.TaskSolutions))
                .ToListAsync();

            List<Subject> userSubjects = User.Identity.GetUserSubjects(subjects).ToList();

            List<SubjectSection> userSubjectSections = userSubjects.SelectMany(x => x.SubjectSections).ToList();

            return View(userSubjectSections);
        }

        // GET: SubjectDivisions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectDivision subjectDivision = await db.SubjectDivisions
                .Include(x => x.FromSubjectSection.FromSubject)
                .Include(x => x.SubjectDivisionChilds)
                .FirstOrDefaultAsync(x => x.Id == id.Value);

            if (subjectDivision == null)
            {
                return HttpNotFound();
            }
            return View(subjectDivision);
        }

        #region Create methods
        // GET: SubjectDivisions/Create
        public ActionResult Create(int? toSubjectSectionId = null)
        {
            ViewBag.toSubjectSectionId = toSubjectSectionId;

            ViewBag.SubjectSectionId = new SelectList(db.SubjectSections, "Id", "Name");
            return View();
        }

        // POST: SubjectDivisions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,SubjectSectionId")] SubjectDivision subjectDivision)
        {
            if (ModelState.IsValid)
            {
                db.SubjectDivisions.Add(subjectDivision);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectSectionId = new SelectList(db.SubjectSections, "Id", "Name", subjectDivision.SubjectSectionId);
            return View(subjectDivision);
        }

        #endregion

        #region Edit methods
        // GET: SubjectDivisions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SubjectDivision subjectDivision = await db.SubjectDivisions
                .Include(x => x.FromSubjectSection.FromSubject)
                .Include(x => x.SubjectDivisionChilds)
                .FirstOrDefaultAsync(x => x.Id == id.Value);
            if (subjectDivision == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectSectionId = new SelectList(db.SubjectSections, "Id", "Name", subjectDivision.SubjectSectionId);
            return View(subjectDivision);
        }

        // POST: SubjectDivisions/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,SubjectSectionId")] SubjectDivision subjectDivision)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subjectDivision).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectSectionId = new SelectList(db.SubjectSections, "Id", "Name", subjectDivision.SubjectSectionId);
            return View(subjectDivision);
        }

        #endregion

        #region Delete methods
        // GET: SubjectDivisions/Delete/5
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
            SubjectDivision subjectDivision = await db.SubjectDivisions.FindAsync(id);
            if (subjectDivision == null)
            {
                return HttpNotFound();
            }
            return View(subjectDivision);
        }

        // POST: SubjectDivisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WorkerResult hasRights = UserHasRightsToBeThere();
            if (!hasRights.Succeeded)
            {
                return RedirectToAction("Index");
            }

            SubjectDivision subjectDivision = await db.SubjectDivisions.FindAsync(id);
            db.SubjectDivisions.Remove(subjectDivision);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #endregion

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
