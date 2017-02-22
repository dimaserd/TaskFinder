using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TwoStu.Logic;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models.Search;
using TwoStu.Logic.Workers;
using System.Data.Entity;
using System.Threading.Tasks;
using System;

namespace TwoStuWeb.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("Solutions")]
    public class SolutionsController : ApiController
    {
        [HttpGet]
        //[Route("Search/{desc:string}")]
        public IEnumerable<TaskSearchResult> Index(string desc, bool markText = false)
        {
            DateTime start = DateTime.Now;
            using (SearchWorker searcher = new SearchWorker(new MyDbContext()))
            {
                var result = searcher.SearchByTaskDesc(desc: desc, searchFile: false, markText: markText);
                DateTime finish = DateTime.Now;
                int miliSecs = (finish - start).Milliseconds;
                System.Diagnostics.Debug.WriteLine(miliSecs);
                return result.OrderByDescending(x => x.DescPercentage);
            }
        }

        [HttpGet]
        [Route("Search/{desc}")]
        public IEnumerable<TaskSearchResult> SearchByDesc(string desc)
        {
            using (SearchWorker searcher = new SearchWorker(new MyDbContext()))
            {
                var result =  searcher.SearchByTaskDesc(desc);
                return result;
            }
        }

        [HttpGet]
        [Route("Subjects")]
        public async Task<IEnumerable<Subject>> GetSubjects()
        {
            using (MyDbContext db = new MyDbContext())
            {
                return await db.Subjects.Include(x => x.SubjectSections).ToListAsync();
            }
        }

        [HttpGet]
        [Route("SubjectSections")]
        public async Task<IEnumerable<SubjectSection>> GetSubjectSections()
        {
            using (MyDbContext db = new MyDbContext())
            {
                return await db.SubjectSections.ToListAsync();
            }
        }
    }
}
