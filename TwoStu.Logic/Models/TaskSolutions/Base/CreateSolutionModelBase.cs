using Extensions.HttpPostedFileBases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Workers;

namespace TwoStu.Logic.Models.TaskSolutions.Base
{

    public class CreateSolutionModelBase
    {
        #region Свойства
        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int SubjectSectionId { get; set; }

        [Required]
        public int WorkTypeId { get; set; }



        [Required]
        public string DivisionChildsString { get; set; }

        [Required]
        [Display(Name = "Условие задачи")]
        public string TaskDesc { get; set; }

        [Required]
        [Display(Name = "Файл решения")]
        public HttpPostedFileBase File { get; set; }

        #endregion

        #region Методы
        public List<SubjectDivisionChild> GetSubjectDivisions(IEnumerable<SubjectDivisionChild> allChilds)
        {
            
            return allChilds.GetSubjectDivisionsFromString(this.DivisionChildsString).ToList();
        }
        #endregion
    }

    #region Расширения
    public static class CreateSolutionModelExtensions
    {
        public static TaskSolution ToTaskSolution(this CreateSolutionModelBase model, string textFromFile)
        {
            return new TaskSolution
            {
                Id = Guid.NewGuid().ToString(),
                SubjectId = model.SubjectId,
                SubjectSectionId = model.SubjectSectionId,
                CreationDate = DateTime.Now,

                FileName = model.File.FileName,
                //Mark = mark,
                TaskDesc = model.TaskDesc,
                WorkTypeId = model.WorkTypeId,
                TrimmedTaskDesc = StringWorker.RemoveSymbols(model.TaskDesc).ToLowerInvariant(),
                TaskDescFromFile = textFromFile,
                //то записываем без слов полученных из файла
                //FilePath = filePath,

            };
        }

        public static TaskSolutionVersion ToTaskSolutionVersion(this CreateSolutionModelBase model, string textFromFile, bool isActive = true)
        {
            return new TaskSolutionVersion
            {
                Id = 0,
                FileData = HttpPostedFileBaseExtensions.GetData(model.File),
                FileMymeType = model.File.ContentType,
                FileName = model.File.FileName,
                FilePath = null,
                IsActive = isActive,
                TaskDesc = model.TaskDesc,
                TaskDescFromFile = textFromFile,
                TrimmedTaskDesc = StringWorker.RemoveSymbols(model.TaskDesc).ToLowerInvariant(),
                VersionDate = DateTime.Now,
            };
        }
    }
    #endregion
}
