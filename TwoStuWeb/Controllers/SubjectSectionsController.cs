using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;

namespace TwoStuWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubjectSectionsController : Controller
    {
        private MyDbContext db = new MyDbContext();

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
                .Include(x => x.SubjectDivisions)
                .Include(x => x.FromSubject)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (subjectSection == null)
            {
                return HttpNotFound();
            }
            return View(subjectSection);
        }

        // GET: SubjectSections/Create
        public ActionResult Create()
        {
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

        // GET: SubjectSections/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
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

        // GET: SubjectSections/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
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
            SubjectSection subjectSection = await db.SubjectSections.FindAsync(id);
            db.SubjectSections.Remove(subjectSection);
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
