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
    public class DivisionChildsController : Controller
    {
        #region Fields
        private MyDbContext db = new MyDbContext();
        #endregion

        #region HttpController methods
        // GET: DivisionChilds
        public async Task<ActionResult> Index()
        {
            var subjectDivisionChilds = db.SubjectDivisionChilds.Include(s => s.SubjectDivisionParent);
            return View(await subjectDivisionChilds.ToListAsync());
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
            WorkerResult hasRights = UserHasRightsToBeThere();
            if(!hasRights.Succeeded)
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
            WorkerResult hasRights = UserHasRightsToBeThere();
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
    }
        #endregion
}
