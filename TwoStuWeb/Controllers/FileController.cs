using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;

namespace TwoStuWeb.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        [HttpGet]
        public ActionResult DownloadByKey(string key)
        {
            if(!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
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