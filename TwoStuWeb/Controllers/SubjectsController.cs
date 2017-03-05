using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models.WorkerResults;
using TwoStu.Logic.Models;

namespace TwoStuWeb.Controllers
{
    [Authorize]
    public class SubjectsController : Controller
    {
        #region Fields
        private MyDbContext db = new MyDbContext();
        #endregion

        #region HttpController methods
        // GET: Subjects
        public async Task<ActionResult> Index()
        {
            return View(await db.Subjects.ToListAsync());
        }

        // GET: Subjects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null || !User.Identity.GetSubjectIds().ToList().Contains(id.Value))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Subject subject = await db.Subjects
                .Include(x => x.SubjectSections.Select(y => y.TaskSolutions))
                .Include(x => x.TaskSolutions)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        #region Create methods
        // GET: Subjects/Create
        public ActionResult Create()
        {
            WorkerResult hasRights = UserHasRightsToBeThere();
            if(!hasRights.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Subjects/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Subject subject)
        {
            WorkerResult hasRights = UserHasRightsToBeThere();
            if (!hasRights.Succeeded)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                subject.Id = db.Subjects.Count() + 1;
                db.Subjects.Add(subject);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(subject);
        }

        #endregion

        #region Edit methods
        // GET: Subjects/Edit/5
        public async Task<ActionResult> Edit(int? id)
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

            Subject subject = await db.Subjects.FindAsync(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Subject subject)
        {
            WorkerResult hasRights = UserHasRightsToBeThere();
            if (!hasRights.Succeeded)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                db.Entry(subject).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(subject);
        }

        #endregion

        #region Delete methods
        // GET: Subjects/Delete/5
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
            Subject subject = await db.Subjects
                .Include(x => x.SubjectSections)
                .Include(x => x.TaskSolutions)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WorkerResult hasRights = UserHasRightsToBeThere();
            if (!hasRights.Succeeded)
            {
                return RedirectToAction("Index");
            }

            Subject subject = await db.Subjects.FindAsync(id);
            db.Subjects.Remove(subject);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #endregion


        #region Help Methods
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
