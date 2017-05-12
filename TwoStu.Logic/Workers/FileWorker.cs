using Extensions.String;
using System;
using System.IO;
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

        #region Свойства
        string FilesDirectory { get; set; }
        #endregion

        #region Публичные методы
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

        public string GetTextFromFile(HttpPostedFileBase file)
        {
            string uniqueName = MakeUniqueFileName(file);

            while (File.Exists($"{FilesDirectory}/{uniqueName}"))
            {
                uniqueName = MakeUniqueFileName(file);
            }

            string filePath = $"{FilesDirectory}/{uniqueName}";
            file.SaveAs(filePath);

            string textInFile = MainWordsExtracter.GetTextFromFile(filePath);

            //удаляю файл так как он больше не нужен
            File.Delete(filePath);

            //возвращаю текст из файла
            return textInFile;
        }
        #endregion

        #region Вспомогательные методы
        string MakeUniqueFileName(HttpPostedFileBase file)
        {
            return $"{Guid.NewGuid()}.{ file.FileName.GetExtensionFromFileName()}";
        }
        #endregion
    }
}
