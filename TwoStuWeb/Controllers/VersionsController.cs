using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;

namespace TwoStuWeb.Controllers
{
    public class VersionsController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Versions
        public async Task<ActionResult> Index()
        {
            return View(await db.TaskSolutionVersions.ToListAsync());
        }

        // GET: Versions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskSolutionVersion taskSolutionVersion = await db.TaskSolutionVersions.FindAsync(id);
            if (taskSolutionVersion == null)
            {
                return HttpNotFound();
            }
            return View(taskSolutionVersion);
        }

        #region Методы создания

        #region Create методы
        // GET: Versions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Versions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,IsActive,VersionDate,TaskDesc,TaskDescFromFile,TrimmedTaskDesc,FilePath,FileName,FileData,FileMymeType")] TaskSolutionVersion taskSolutionVersion)
        {
            if (ModelState.IsValid)
            {
                db.TaskSolutionVersions.Add(taskSolutionVersion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(taskSolutionVersion);
        }

        #endregion

        #region Создание новой версии к решении
        public async Task<ActionResult> AddVersion(string id)
        {
            TaskSolution solution = await db.TaskSolutions.FirstOrDefaultAsync(x => x.Id == id);


        }

        #endregion
        #endregion

        // GET: Versions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskSolutionVersion taskSolutionVersion = await db.TaskSolutionVersions.FindAsync(id);
            if (taskSolutionVersion == null)
            {
                return HttpNotFound();
            }
            return View(taskSolutionVersion);
        }

        // POST: Versions/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,IsActive,VersionDate,TaskDesc,TaskDescFromFile,TrimmedTaskDesc,FilePath,FileName,FileData,FileMymeType")] TaskSolutionVersion taskSolutionVersion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taskSolutionVersion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(taskSolutionVersion);
        }

        // GET: Versions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskSolutionVersion taskSolutionVersion = await db.TaskSolutionVersions.FindAsync(id);
            if (taskSolutionVersion == null)
            {
                return HttpNotFound();
            }
            return View(taskSolutionVersion);
        }

        // POST: Versions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TaskSolutionVersion taskSolutionVersion = await db.TaskSolutionVersions.FindAsync(id);
            db.TaskSolutionVersions.Remove(taskSolutionVersion);
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
