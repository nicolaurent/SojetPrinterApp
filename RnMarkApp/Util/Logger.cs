using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnMarkApp.Util
{
    public static class Logger
    {
        private static string logPath = Directory.GetCurrentDirectory() + "/Log/";
        private static string fileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "_" +
                DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "_Logs.txt";

        private static string currentDate = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();

        public static void VerifyDir(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                if (!dir.Exists)
                {
                    dir.Create();
                }

                // Change to different log file when date changed
                if(currentDate != DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())
                {
                    fileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "_" +
                DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "_Logs.txt";
                }

            }
            catch (Exception e) { }
        }

        public static void Info(string lines)
        {
            VerifyDir(logPath);
            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(logPath + fileName, true);
                file.WriteLine(DateTime.Now.ToString() + ": INFO: " + lines);
                file.Close();
            }
            catch (Exception) { }
        }

        public static void Error(string lines)
        {
            VerifyDir(logPath);
            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(logPath + fileName, true);
                file.WriteLine(DateTime.Now.ToString() + ": ERROR: " + lines);
                file.Close();
            }
            catch (Exception) { }
        }
    }
}
