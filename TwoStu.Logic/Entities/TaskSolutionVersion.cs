using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using TwoStu.Logic.Models;
using TwoStu.Logic.Workers;

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

    #region Расширения
    public static class TaskSolutionVersionExtensions
    {
        public static TaskSolutionVersion ToTaskSolutionVersion(this CreatePhysicsSolutionModel model, string textFromFile, bool isActive = true)
        {
            return new TaskSolutionVersion
            {
                Id = 0,
                FileData = GetDataFromHttpPostedFileBase(model.File),
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        static byte[] GetDataFromHttpPostedFileBase(HttpPostedFileBase file)
        {
            byte[] myFile;
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                myFile = memoryStream.ToArray();// or use .GetBuffer() as suggested by Morten Anderson
                return myFile;
            }
        }
    }

    #endregion
}
