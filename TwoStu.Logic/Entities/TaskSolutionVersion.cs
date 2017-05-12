using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;
using TwoStu.Logic.Models;
using TwoStu.Logic.Workers;

namespace TwoStu.Logic.Entities
{
    public class TaskSolutionVersion
    {
        [Key]
        public int Id { get; set; }

        
        [JsonIgnore]
        public virtual TaskSolution FromTaskSolution { get; set; }


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

        public byte[] FileData { get; set; }

        public string FileMymeType { get; set; }
        /// <summary>
        /// Списки уточнений по данному предмету должны находиться здесь так как
        /// система должна быть гибкой ко всем этим изменениям в списке уточнений
        /// </summary>
        public virtual IList<SubjectDivisionChild> SubjectDivisionChilds { get; set; }

    }

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
}
