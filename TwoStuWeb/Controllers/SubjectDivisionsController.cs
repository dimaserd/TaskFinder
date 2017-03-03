using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models.WorkerResults;

namespace TwoStuWeb.Controllers
{
    [Authorize]
    public class SubjectDivisionsController : Controller
    {
        private MyDbContext db = new MyDbContext();

        #region HttpController methods
        // GET: SubjectDivisions
        public async Task<ActionResult> Index()
        {
            var subjectDivisions = db.SubjectDivisions.Include(s => s.FromSubjectSection);
            return View(await subjectDivisions.ToListAsync());
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
            WorkerResult hasRights = UserHasRightsToBeThere();
            if(!hasRights.Succeeded)
            {
                return RedirectToAction("Index");
            }

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
