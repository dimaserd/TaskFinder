using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoStu.Logic.Entities
{
    public class TaskSolution
    {
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

        #region Foreign Keys Properties
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
        public virtual ICollection<SubjectDivisionChild> SubjectDivisionChilds { get; set; }
        #endregion


    }


}
