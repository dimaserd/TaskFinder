using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwoStu.Logic.Entities
{
    public class TaskSolutionVersion
    {
        #region Свойства
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Флаг по которому будет определяться является ли данная версия решения активной
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Дата добавления новой версии
        /// </summary>
        public DateTime VersionDate { get; set; }

        /// <summary>
        /// Описание задания. Условие выполненной задачи.
        /// </summary>
        public string TaskDesc { get; set; }

        /// <summary>
        /// Условие задания взятое из файла
        /// </summary>
        public string TaskDescFromFile { get; set; }

        /// <summary>
        /// Вытащенный текст из файла решения. (нуждается в доработке)
        /// </summary>
        public string TrimmedTaskDesc { get; set; }

        #region Свойства для файла
        /// <summary>
        /// Путь к физическому файлу в системе. (Более не используется).
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Имя файла. Используется для возвращения файла с нужным расширением из массива байтов.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Содержимое файла решения в байтах.
        /// </summary>
        public byte[] FileData { get; set; }


        /// <summary>
        /// Майм тип файла. (используется для доставания содержимого файла обратно из массива байтов)
        /// </summary>
        public string FileMymeType { get; set; }
        
        #endregion

        #region Свойства отношений 
        /// <summary>
        /// Списки уточнений по данному предмету.
        /// </summary>
        public virtual IList<SubjectDivisionChild> SubjectDivisionChilds { get; set; }

        [JsonIgnore]
        public virtual TaskSolution FromTaskSolution { get; set; }

        #endregion

        #endregion
    }

    
}
