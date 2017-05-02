using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;
using System.Data.Entity;

namespace TwoStuWeb.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        [HttpGet]
        public ActionResult DownloadByKey(string key, bool fromVersion = false)
        {
            if(!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if(fromVersion)
            {
                using (MyDbContext context = new MyDbContext())
                {
                    TaskSolution solution = context.TaskSolutions.Include(t => t.Versions).FirstOrDefault(t => t.Id == key);
                    if (solution == null || solution.Versions.Count == 0)
                    {
                        return new HttpStatusCodeResult(404);
                    }

                    TaskSolutionVersion version = solution.Versions.FirstOrDefault(t => t.IsActive);

                    if(version == null)
                    {
                        return new HttpStatusCodeResult(404);
                    }

                    return File(version.FileData, version.FileMymeType, version.FileName);
                }
            }

            using (MyDbContext context = new MyDbContext())
            {
                TaskSolution a = context.TaskSolutions.FirstOrDefault(t => t.Id == key);
                if (a == null || !System.IO.File.Exists(a.FilePath))
                {
                    return new HttpStatusCodeResult(404);
                }

                return File(a.FilePath, MimeMapping.GetMimeMapping(a.FilePath), a.FileName);
            }
        }


    }
}