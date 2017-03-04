using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models.TaskSolutions;

namespace TwoStu.Logic.Workers.TaskSolutions
{
    public class TaskSolutionSearcher
    {
        #region Constructor
        public TaskSolutionSearcher(MyDbContext db)
        {
            Db = db;
        }
        #endregion

        #region Properties
        public MyDbContext Db { get; set; }
        #endregion

        #region Public Methods
        public IEnumerable<TaskSolution> SearchTasks(SearchSolutionsModel model)
        {
            List<SubjectDivisionChild> childsFromString = Db.SubjectDivisionChilds
                .ToList()
                .GetSubjectDivisionsFromString(model.DivisionChildsString)
                .ToList();

            //получили все данные и материализовали коллекцию
            List<TaskSolution> solutions = Db.TaskSolutions
                .Where(x => x.SubjectId == model.SubjectId)
                //.Where(x => x.SubjectSectionId == model.SubjectSectionId)
                .Where(x => x.WorkTypeId == model.WorkTypeId)
                .Include(x => x.SubjectDivisionChilds.Select(y => y.SubjectDivisionParent))
                .ToList();

            if(model.SubjectSectionId > 0)
            {
                solutions = solutions.Where(x => x.SubjectSectionId == model.SubjectSectionId).ToList();
            }
            
            if(!string.IsNullOrEmpty(model.TaskDesc))
            {
                solutions = solutions.Where(x => x.TaskDesc.Contains(model.TaskDesc)).ToList();
            }

            if(childsFromString.Count > 0)
            {
                solutions = solutions.Where(x => x.SubjectDivisionChilds.Any(y => childsFromString.Any(z => z.Id == y.Id)))
                .ToList();
            }

            return solutions;
        }
        #endregion

        #region Help Methods
        List<SubjectDivision> GetAllSubjectDivisionsWithChilds()
        {
            return Db.SubjectDivisions
                .Include(x => x.SubjectDivisionChilds)
                .ToList();
        }
        #endregion
    }
}
