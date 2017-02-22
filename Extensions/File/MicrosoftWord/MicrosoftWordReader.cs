using Novacode;

namespace Extensions.File.MicrosoftWord
{
    public static class MicrosoftWordReader
    {
        public static string ReadTextFromDocxFile(string docxPath)
        {
            using (DocX d = DocX.Load(docxPath))
            {
                return d.Text;
            }
        }
    }
}
