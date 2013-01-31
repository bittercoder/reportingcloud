using System.IO;

namespace ReportingCloud.Engine.Loader
{
    public class RdlSourceLoader : IRdlSourceLoader
    {
        public string GetRdlSource(string path)
        {
            StreamReader fs = null;
            string prog = null;
            try
            {
                fs = new StreamReader(path);
                prog = fs.ReadToEnd();
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

            return prog;
        }
    }
}