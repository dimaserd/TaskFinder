using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TwoStu.Logic.Entities;
using System.Data.Entity;

namespace TwoStu.Logic.Workers.Temporary
{
    public class TaskSolutionsFixer
    {

        #region Поля
        MyDbContext _db;

        int countSetting = 250;

        #endregion

         MyDbContext Db
        {
            get
            {
                if(_db == null)
                {
                    _db = new MyDbContext();
                }
                return _db;
            }
        }

        public int GetVersionsMinusSolutions()
        {
            List<TaskSolution> solutions = Db.TaskSolutions.ToList();

            List<TaskSolutionVersion> solutionVersions = Db.TaskSolutionVersions.ToList();

            return (solutionVersions.Count - solutions.Count);
        }

        public void DropVersions()
        {
            Db.TaskSolutionVersions.RemoveRange(Db.TaskSolutionVersions.ToList());
            
            Db.SaveChanges();
        }

        public List<TaskSolution> GetSolutionsWithNoFiles()
        {
            List<TaskSolution> solutions = Db.TaskSolutions.ToList();

            return solutions.Where(x =>
            {
                return File.Exists(x.FilePath);
            }).ToList();
        }

        public int FixSolutions()
        {
            List<TaskSolution> solutions = Db.TaskSolutions.Include(x => x.Versions).ToList()
                .Where(x => x.Versions.Count == 0).ToList().Take(countSetting).ToList();

            int count = 0;
            
            for(int i = 0; i < solutions.Count; i++)
            {

                TaskSolutionVersion solutionVersion = GetTaskSolutionVersion(solutions[i]);

                if (solutionVersion != null)
                {
                    count++;
                    Db.TaskSolutionVersions.Add(solutionVersion);
                }
            }

            Db.SaveChanges();

            return count;
        }

        public IEnumerable<TaskSolution> GetNotLocalSolutions()
        {
            List<TaskSolution> solutions = Db.TaskSolutions.ToList();

            return solutions.Select(x =>
            {
                if (!File.Exists(x.FilePath))
                {
                    return x;
                }
                else
                {
                    return null;
                }
            }).Where(x => x != null);
        }

        #region Вспомогательные методы    
        

        TaskSolutionVersion GetTaskSolutionVersion(TaskSolution solution)
        {
            if(solution == null)
            {
                throw new System.Exception("solution is null!");

            }

            //если файл не существует то
            if (!File.Exists(solution.FilePath))
            {
                return null;
            }

            return new TaskSolutionVersion
            {
                Id = 0,
                VersionDate = DateTime.Now,
                SubjectDivisionChilds = null,
                FromTaskSolution = solution,
                
                TaskDesc = solution.TaskDesc,
                TaskDescFromFile = solution.TaskDescFromFile,
                IsActive = true,
                //по файлу
                FileName = solution.FileName,
                FileData = File.ReadAllBytes(solution.FilePath),
                FileMymeType = MimeMapping.GetMimeMapping(solution.FilePath),
                FilePath = solution.FilePath,
               
                TrimmedTaskDesc = solution.TrimmedTaskDesc,
            };
        }

        #endregion
    }
}
