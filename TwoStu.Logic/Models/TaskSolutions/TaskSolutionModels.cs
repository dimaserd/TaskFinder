using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Workers;

namespace TwoStu.Logic.Models
{
    #region Subject Types Enums
    public enum PhysicsType
    {
        Mechanics, Kinеmatics, ThermoDynamics,
    }
    #endregion

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

    public static class CreatePhysicsSolutionModelExtensions
    {
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
    }


}
