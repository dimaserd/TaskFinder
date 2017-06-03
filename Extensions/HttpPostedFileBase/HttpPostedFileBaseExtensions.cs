using System.IO;
using System.Web;

namespace Extensions.HttpPostedFileBases
{
    public static class HttpPostedFileBaseExtensions
    {
        public static byte[] GetData(HttpPostedFileBase file)
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
