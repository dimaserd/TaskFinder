using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TwoStu.Logic.Entities;

namespace TwoStu.Logic.Workers.Temporary
{
    public class TaskSolutionsFixer
    {
        public MyDbContext Db = new MyDbContext();

        public void FixSolutions()
        {
            List<TaskSolution> solutions = Db.TaskSolutions.ToList();
            
            
            foreach (TaskSolution solution in solutions)
            {

                TaskSolutionVersion solutionVersion = GetTaskSolutionVersion(solution);
                Db.TaskSolutionVersions.Add(solutionVersion);
            }

            Db.SaveChanges();
        }

        /// <summary>
        /// Не добавляет в базу
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        TheFile GetFileFromSolution(TaskSolution solution)
        {
            if(solution == null)
            {
                throw new System.Exception("solution is null");
            }

            
            return new TheFile
            {
                Id = 0,
                Name = solution.FileName,
                FileData = File.ReadAllBytes(solution.FilePath),
                FileMymeType = MimeMapping.GetMimeMapping(solution.FilePath),
            };

            
        }

        TaskSolutionVersion GetTaskSolutionVersion(TaskSolution solution)
        {
            if(solution == null)
            {
                throw new System.Exception("solution is null!");

            }

            return new TaskSolutionVersion
            {
                Id = 0,
                VersionDate = DateTime.Now,
                SubjectDivisionChilds = null,
                File = GetFileFromSolution(solution),
                TaskSolutionId = solution.Id,
                TaskDesc = solution.TaskDesc,
                TaskDescFromFile = solution.TaskDescFromFile,
                IsActive = true,
                FilePath = solution.FilePath,
                FileName = solution.FileName,
                TrimmedTaskDesc = solution.TrimmedTaskDesc,
            };
        }
    }
}
