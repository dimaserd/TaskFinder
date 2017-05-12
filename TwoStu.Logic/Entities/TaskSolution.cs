using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TwoStu.Logic.Models.TaskSolutions;

namespace TwoStu.Logic.Entities
{
    public class TaskSolution
    {
        #region Конструкторы
        public TaskSolution()
        {
            SubjectDivisionChilds = new List<SubjectDivisionChild>();
            Versions = new List<TaskSolutionVersion>();
        }

        #endregion

        [Key]
        public string Id { get; set; }

        //в базе храняться уточнения и сами предметы
        //с помощбю конкатенации строк {предмет}{уточнение}
        //мы будем получать нужные нам результаты в поиске
        //и сохраним целостность в базе
        public string Mark { get; set; }

        public DateTime CreationDate { get; set; }

        public string TaskDesc { get; set; }

        public string TaskDescFromFile { get; set; }

        public string TrimmedTaskDesc { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        #region Отношения к коллекциям

        [Required]
        [ForeignKey("TypeOfWork")]
        public int WorkTypeId { get; set; }
        [JsonIgnore]
        public virtual WorkType TypeOfWork { get; set; }

        [Required]
        [ForeignKey("TaskSubject")]
        public int SubjectId { get; set; }
        [JsonIgnore]
        public virtual Subject TaskSubject { get; set; }


        [Required]
        [ForeignKey("TaskSubjectSection")]
        public int SubjectSectionId { get; set; }
        [JsonIgnore]
        public virtual SubjectSection TaskSubjectSection { get; set; }


        /// <summary>
        /// Сущности TaskSolution и SubjectDivisionChild связаны многое ко многим
        /// </summary>
        public virtual IList<SubjectDivisionChild> SubjectDivisionChilds { get; set; }

        /// <summary>
        /// Версии данного решения могут изменяться например может стать другим описание
        /// задачи и так далее
        /// </summary>
        public virtual IList<TaskSolutionVersion> Versions { get; set; }

        #endregion


    }

    public static class TaskSolutionExtensions
    {
        public static string GetLinkToDownload(this TaskSolution solution)
        {
            return $"/File/DownloadByKey?key={solution.Id}";
        }

        public static MakeTaskSolutionVersionModel GetModelForCreatingVersion(this TaskSolution solution, List<SubjectDivision> subjectDivisionsWithChilds)
        {
            return new MakeTaskSolutionVersionModel
            {
                TaskSolutionId = solution.Id,
                VersionDate = DateTime.Now,
                ExistingVersions = solution.Versions.ToList(),

                SubjectDivisionsWithChilds = subjectDivisionsWithChilds,
            };
        }

        public static TaskSolution ToTaskSolution(this CreateSolutionModel model, List<SubjectDivisionChild> solutionDivisionChilds, string textInFile)
        {
            return new TaskSolution
            {
                Id = Guid.NewGuid().ToString(),
                CreationDate = DateTime.Now,
                TaskDesc = model.TaskDesc,
                SubjectId = model.SubjectId,
                SubjectSectionId = model.SubjectSectionId,
                WorkTypeId = model.WorkTypeId,
                //FilePath = filePath,
                FileName = model.File.FileName,
                SubjectDivisionChilds = solutionDivisionChilds,
                //Mark = null,
                TaskDescFromFile = textInFile,
            };
        }
    }
}
