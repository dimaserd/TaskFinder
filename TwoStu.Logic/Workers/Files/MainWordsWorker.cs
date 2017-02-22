using Extensions.String;
using System.Collections.Generic;
using System.Linq;

namespace TwoStu.Logic.Workers.Files
{
    public static class MainWordsExtracter
    {
        //метод получает файл любого типа и пытается из него достать условие
        //задачи
        public static List<string> GetWordsFromFile(string anyTypeFilePath)
        {
            return GetTextFromFile(anyTypeFilePath).GetWordsFromText();
        }

        public static string GetTextFromFile(string anyTypeFilePath)
        {
            string ext = anyTypeFilePath.GetExtensionFromFileName();

            switch (ext)
            {
                //на данный момент программа может извлекать текст 
                //только из вордовских документов
                case "docx":
                    return MicrosoftOfficeWordFileWorker.GetTextFromDocxFile(anyTypeFilePath);


                default:
                    return null;

            }
        }
    }
}
