using Extensions.String;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using TwoStu.Logic.Workers.Files;

namespace TwoStu.Logic.Workers
{
    public class FileWorker
    {
        #region Constructors
        public FileWorker()
        {
            Preparations();
        }

        void Preparations()
        {
            FilesDirectory = HostingEnvironment.MapPath("~/Files");

            if(!Directory.Exists(FilesDirectory))
            {
                Directory.CreateDirectory(FilesDirectory);
            }
        }
        #endregion

        #region Properties
        string FilesDirectory { get; set; }
        #endregion

        #region Public Methods
        //метод сохраняет файл и возвращает путь к файлу
        //также пытается достать из него слова
        public string SaveFileToSolution(HttpPostedFileBase file, out string textInFile)
        {
            string uniqueName = MakeUniqueFileName(file);

            while(File.Exists($"{FilesDirectory}/{uniqueName}"))
            {
                uniqueName = MakeUniqueFileName(file);
            }

            string filePath = $"{FilesDirectory}/{uniqueName}";
            file.SaveAs(filePath);

            textInFile = MainWordsExtracter.GetTextFromFile(filePath);
            return filePath;
        }
        #endregion

        #region Help Methods
        string MakeUniqueFileName(HttpPostedFileBase file)
        {
            return $"{Guid.NewGuid()}.{ file.FileName.GetExtensionFromFileName()}";
        }
        #endregion
    }
}
