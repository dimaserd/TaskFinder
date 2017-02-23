using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;

namespace TwoStuWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubjectDivisionsController : Controller
    {
        private MyDbContext db = new MyDbContext();

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
            SubjectDivision subjectDivision = await db.SubjectDivisions.FindAsync(id);
            if (subjectDivision == null)
            {
                return HttpNotFound();
            }
            return View(subjectDivision);
        }

        // GET: SubjectDivisions/Create
        public ActionResult Create()
        {
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

        // GET: SubjectDivisions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
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
            if (ModelState.IsValid)
            {
                db.Entry(subjectDivision).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectSectionId = new SelectList(db.SubjectSections, "Id", "Name", subjectDivision.SubjectSectionId);
            return View(subjectDivision);
        }

        // GET: SubjectDivisions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
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
            SubjectDivision subjectDivision = await db.SubjectDivisions.FindAsync(id);
            db.SubjectDivisions.Remove(subjectDivision);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

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
