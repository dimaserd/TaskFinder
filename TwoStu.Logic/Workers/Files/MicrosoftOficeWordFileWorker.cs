using Extensions.File.MicrosoftWord;
using Extensions.String;
using System;

namespace TwoStu.Logic.Workers.Files
{
    public static class MicrosoftOfficeWordFileWorker
    {
        
        public static string GetTextFromDocxFile(string docxFilePath)
        {
            string ext = docxFilePath.GetExtensionFromFileName();
            if(ext != "docx")
            {
                throw new Exception("Ожидался файл другого формата!");
            }
            return MicrosoftWordReader.ReadTextFromDocxFile(docxFilePath);
        }
    }
}
