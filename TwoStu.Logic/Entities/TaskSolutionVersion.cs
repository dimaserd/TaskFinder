using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoStu.Logic.Entities
{
    public class TaskSolutionVersion
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("FromTaskSolution")]
        public string TaskSolutionId { get; set; }
        [JsonIgnore]
        public virtual TaskSolution FromTaskSolution { get; set; }


        [ForeignKey("File")]
        public int FileId { get; set; }
        [JsonIgnore]
        public virtual TheFile File { get; set; }

        /// <summary>
        /// Флаг по которому будет определяться является ли данная версия решения активной
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Дата добавления новой версии
        /// </summary>
        public DateTime VersionDate { get; set; }

        public string TaskDesc { get; set; }

        public string TaskDescFromFile { get; set; }

        public string TrimmedTaskDesc { get; set; }

        /// <summary>
        /// Если будет создаваться новый файл решения то нужно будет добавлять единичку к новому
        /// то есть создаем версионность
        /// </summary>
        public string FilePath { get; set; }

        public string FileName { get; set; }

        /// <summary>
        /// Списки уточнений по данному предмету должны находиться здесь так как
        /// система должна быть гибкой ко всем этим изменениям в списке уточнений
        /// </summary>
        public virtual IList<SubjectDivisionChild> SubjectDivisionChilds { get; set; }

    }
}
