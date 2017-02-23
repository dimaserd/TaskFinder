using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;

namespace TwoStuWeb.Controllers
{
    public class DivisionChildsController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: DivisionChilds
        public async Task<ActionResult> Index()
        {
            var subjectDivisionChilds = db.SubjectDivisionChilds.Include(s => s.SubjectDivisionParent);
            return View(await subjectDivisionChilds.ToListAsync());
        }

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

        // GET: DivisionChilds/Create
        public ActionResult Create()
        {
            ViewBag.SubjectDivisionId = new SelectList(db.SubjectDivisions, "Id", "Name");
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

        // GET: DivisionChilds/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            if (ModelState.IsValid)
            {
                db.Entry(subjectDivisionChild).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectDivisionId = new SelectList(db.SubjectDivisions, "Id", "Name", subjectDivisionChild.SubjectDivisionId);
            return View(subjectDivisionChild);
        }

        // GET: DivisionChilds/Delete/5
        public async Task<ActionResult> Delete(int? id)
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

        // POST: DivisionChilds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SubjectDivisionChild subjectDivisionChild = await db.SubjectDivisionChilds.FindAsync(id);
            db.SubjectDivisionChilds.Remove(subjectDivisionChild);
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
