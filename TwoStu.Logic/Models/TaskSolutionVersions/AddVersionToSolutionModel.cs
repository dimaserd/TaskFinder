using System;
using System.Collections.Generic;
using System.Web;
using TwoStu.Logic.Entities;

namespace TwoStu.Logic.Models.TaskSolutionVersions
{
    public class AddVersionToSolutionModel
    {
        #region Конструкторы
        public AddVersionToSolutionModel(TaskSolution solution)
        {
            SolutionId = solution.Id;
            VersionDate = DateTime.Now;
            SubjectDivisionChilds = new List<SubjectDivisionChild>();
        }
        #endregion

        #region Свойства
        public string SolutionId { get; set; }

        public DateTime VersionDate { get; set; }

        /// <summary>
        /// Описание задания. Условие выполненной задачи.
        /// </summary>
        public string TaskDesc { get; set; }

        public HttpPostedFileBase SolutionFile { get; set; }

        /// <summary>
        /// Список уточнений по данному предмету
        /// </summary>
        public List<SubjectDivisionChild> SubjectDivisionChilds { get; set; }

        public bool IsActive { get; set; }
        #endregion
    }

    #region Расширения

    public static class AddVersionToSolutionModelExtensions
    {
        public static TaskSolutionVersion ToTaskSolutionVersion(this AddVersionToSolutionModel model)
        {
            return new TaskSolutionVersion
            {
                Id = 0,
                IsActive = model.IsActive,
                TaskDesc = model.TaskDesc,
                SubjectDivisionChilds = model.SubjectDivisionChilds,
                VersionDate = model.VersionDate,
            };
        }
    }
    #endregion
}
