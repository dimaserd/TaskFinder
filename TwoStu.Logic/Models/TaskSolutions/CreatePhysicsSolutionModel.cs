using Extensions.HttpPostedFileBases;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models.TaskSolutions.Base;
using TwoStu.Logic.Workers;

namespace TwoStu.Logic.Models
{

    public class CreatePhysicsSolutionModel
    {
        [Required]
        [Display(Name = "Условие задачи")]
        public string TaskDesc { get; set; }

        [Required]
        [Display(Name = "Раздел физики")]
        public int SubjectSectionId { get; set; }

        [Required]
        [Display(Name = "Тип работы")]
        public int WorkTypeId { get; set; }

        [Required]
        [Display(Name = "Файл решения")]
        public HttpPostedFileBase File { get; set; }
    }

    #region Расширения
    public static class CreatePhysicsSolutionModelExtensions
    {
        public static CreateSolutionModelBase ToCreateSolutionModel(this CreatePhysicsSolutionModel model, int physicsId)
        {
            return new CreateSolutionModelBase
            {
                SubjectId = physicsId,
                TaskDesc = model.TaskDesc,
               
                File = model.File,
                SubjectSectionId = model.SubjectSectionId,
                WorkTypeId = model.WorkTypeId,

                DivisionChildsString = null,

            };
        }

        public static TaskSolution ToTaskSolution(this CreatePhysicsSolutionModel model, int physicsId, string textFromFile)
        {
            return new TaskSolution
            {
                Id = Guid.NewGuid().ToString(),
                SubjectId = physicsId,
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

        public static TaskSolutionVersion ToTaskSolutionVersion(this CreatePhysicsSolutionModel model, string textFromFile, bool isActive = true)
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
