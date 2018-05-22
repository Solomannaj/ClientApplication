using System;
using System.IO;
using System.Web.Hosting;

namespace ClientApplication.BusinessLogic
{
    public class Logger
    {
        public static void WrieException(string exception)
        {
            using (StreamWriter w = File.AppendText(HostingEnvironment.ApplicationPhysicalPath + @"\Loggs\log.txt"))
            {
                Log(exception, w);
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }
    }
}