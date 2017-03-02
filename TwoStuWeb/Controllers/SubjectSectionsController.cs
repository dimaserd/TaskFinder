using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models.WorkerResults;

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
            var subjectSections = db.SubjectSections.Include(s => s.FromSubject);
            return View(await subjectSections.ToListAsync());
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
        public ActionResult Create()
        {
            WorkerResult result = UserHasRightsToBeThere();
            if (!result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name");
            return View();
        }

        // POST: SubjectSections/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,SubjectId")] SubjectSection subjectSection)
        {
            WorkerResult result = UserHasRightsToBeThere();
            if (!result.Succeeded)
            {
                return RedirectToAction("Index");
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
            WorkerResult result = UserHasRightsToBeThere();
            if (!result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectSection subjectSection = await db.SubjectSections.FindAsync(id);
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
            SubjectSection subjectSection = await db.SubjectSections.FindAsync(id);
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
