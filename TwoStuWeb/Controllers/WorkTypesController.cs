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
    public class WorkTypesController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: WorkTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.WorkTypes.ToListAsync());
        }

        // GET: WorkTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkType workType = await db.WorkTypes.FindAsync(id);
            if (workType == null)
            {
                return HttpNotFound();
            }
            return View(workType);
        }

        // GET: WorkTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkTypes/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] WorkType workType)
        {
            if (ModelState.IsValid)
            {
                workType.Id = db.WorkTypes.Count() + 1;
                db.WorkTypes.Add(workType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(workType);
        }

        // GET: WorkTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkType workType = await db.WorkTypes.FindAsync(id);
            if (workType == null)
            {
                return HttpNotFound();
            }
            return View(workType);
        }

        // POST: WorkTypes/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] WorkType workType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(workType);
        }

        // GET: WorkTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkType workType = await db.WorkTypes.FindAsync(id);
            if (workType == null)
            {
                return HttpNotFound();
            }
            return View(workType);
        }

        // POST: WorkTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WorkType workType = await db.WorkTypes.FindAsync(id);
            db.WorkTypes.Remove(workType);
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
