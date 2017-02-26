using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using TwoStu.Logic;
using TwoStu.Logic.Entities;
using System.Data.Entity;
using System.Linq;
using TwoStu.Logic.Models.Api;

namespace TwoStuWeb.Controllers.api
{
    [RoutePrefix("Default")]
    public class DefaultController : ApiController
    {
        [HttpGet]
        [Route("GetSubjects")]
        public async Task<SubjectsApiResult> GetSubjects()
        {
            using (MyDbContext db = new MyDbContext())
            {
                return new SubjectsApiResult
                {
                    SubjectDivisionChilds = await db.SubjectDivisionChilds.ToListAsync(),
                    SubjectDivisions = await db.SubjectDivisions.ToListAsync(),
                    Subjects = await db.Subjects.ToListAsync(),
                    SubjectSections = await db.SubjectSections.ToListAsync()
                    
                };

            }
        }

        [HttpGet]
        [Route("GetSubjectSections")]
        public async Task<IEnumerable<SubjectSection>> GetSubjectSections(int? subjectId = null)
        {
            using (MyDbContext db = new MyDbContext())
            {
                if(!subjectId.HasValue)
                {
                    return await db.SubjectSections.ToListAsync();
                }
                else
                {
                    return await db.SubjectSections
                        .Where(x => x.SubjectId == subjectId.Value)
                        .ToListAsync();
                }
                
            }
        }

        [HttpGet]
        [Route("GetSubjectDivisions")]
        public async Task<IEnumerable<SubjectDivision>> GetSubjectDivisions(int? subjectSectionId = null)
        {
            using (MyDbContext db = new MyDbContext())
            {
                if(!subjectSectionId.HasValue)
                {
                    return await db.SubjectDivisions.ToListAsync();
                }
                else
                {
                    return await db.SubjectDivisions
                        .Where(x => x.SubjectSectionId == subjectSectionId.Value)
                        .ToListAsync();
                }
                
            }
        }

        [HttpGet]
        [Route("GetSubjectDivisionChilds")]
        public async Task<IEnumerable<SubjectDivisionChild>> GetSubjectDivisionChilds(int? subjectDivisionId = null)
        {
            using (MyDbContext db = new MyDbContext())
            {
                if (!subjectDivisionId.HasValue)
                {
                    return await db.SubjectDivisionChilds.ToListAsync();
                }
                else
                {
                    return await db.SubjectDivisionChilds
                        .Where(x => x.SubjectDivisionId == subjectDivisionId.Value)
                        .ToListAsync();
                }

            }
        }

    }
}
