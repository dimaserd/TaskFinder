using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoStu.Logic.Entities;

namespace TwoStu.Logic.Models.TaskSolutions
{
    /// <summary>
    /// Класс по которому будет создаваться версия решения в системе
    /// и выбор текущей версии
    /// </summary>
    public class MakeTaskSolutionVersionModel
    {
        public string TaskSolutionId { get; set; }

        public DateTime VersionDate { get; set; }

        public string TaskDesc { get; set; }
        
        /// <summary>
        /// По ним нужно сделать выпадающий список выбора активной версии
        /// </summary>
        public List<TaskSolutionVersion> ExistingVersions { get; set; }


        #region Для выбора чего-то из базы
        /// <summary>
        /// Для новой версии должен быть выбор всех уточнений
        /// </summary>
        public List<SubjectDivision> SubjectDivisionsWithChilds { get; set; }
        #endregion
    }
}
