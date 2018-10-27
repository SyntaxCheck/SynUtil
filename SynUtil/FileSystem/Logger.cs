using System;
using System.IO;

namespace SynUtil.FileSystem
{
    public class Logger
    {
        public static void Write(string logPath, string logMessage)
        {
            if (!File.Exists(logPath))
            {
                using (StreamWriter sw = File.CreateText(logPath))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + logMessage);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + logMessage);
                }
            }
        }
        public static void Write(LogInfo logInfo, string logMessage)
        {
            //If we have not validated the path yet then call the ValidatePath. This will build the folder structure if it does not exist
            if (!logInfo.PathValidated)
                ValidateLogPath(ref logInfo);

            Write(logInfo.FullPath, logMessage);
        }
        public static void Write(LogInfo logInfo, string messageHeader, Exception inException)
        {
            Write(logInfo, messageHeader + " Exception Message: " + inException.Message);
            Write(logInfo, messageHeader + " Exception Source: " + inException.Source);
            Write(logInfo, messageHeader + " Exception TargetSite: " + inException.TargetSite);
            Write(logInfo, messageHeader + " Exception Data: " + inException.Data);
            Write(logInfo, messageHeader + " Exception StackTrace: " + inException.StackTrace);
            if (inException.InnerException != null)
            {
                Write(logInfo, messageHeader + " Inner Exception Message: " + inException.InnerException.Message);
                Write(logInfo, messageHeader + " Inner Exception Source: " + inException.InnerException.Source);
                Write(logInfo, messageHeader + " Inner Exception TargetSite: " + inException.InnerException.TargetSite);
                Write(logInfo, messageHeader + " Inner Exception Data: " + inException.InnerException.Data);
                Write(logInfo, messageHeader + " Inner Exception StackTrace: " + inException.InnerException.StackTrace);
            }
        }
        public static void WriteDbg(string logPath, string logMessage, bool isDebug = true)
        {
            if (isDebug)
            {
                Write(logPath, logMessage);
            }
        }
        public static void WriteDbg(LogInfo logInfo, string logMessage)
        {
            if (logInfo.IsDebug)
            {
                Write(logInfo, logMessage);
            }
        }
        public static void WriteDbg(LogInfo logInfo, string messageHeader, Exception inException)
        {
            if (logInfo.IsDebug)
            {
                Write(logInfo, messageHeader, inException);
            }
        }
        public static void ValidateLogPath(ref LogInfo logInfo)
        {
            if (String.IsNullOrEmpty(logInfo.RootFolder) || String.IsNullOrEmpty(logInfo.LogFolder))
            {
                throw new Exception("Log Validate Path: RootFolder and/or LogFolder not provided");
            }
            else
            {
                //Verify that the root folder exists
                if (!Directory.Exists(logInfo.RootFolder))
                {
                    throw new Exception("Log Validate Path: RootFolder (" + logInfo.RootFolder + ") does not exist or we do not have permissions to it");
                }
                else
                {
                    //Build the log subfolder if it does not exist
                    if (!Directory.Exists(Path.Combine(logInfo.RootFolder, logInfo.LogFolder)))
                    {
                        Directory.CreateDirectory(Path.Combine(logInfo.RootFolder, logInfo.LogFolder));
                    }

                    logInfo.PathValidated = true;
                }
            }
        }
    }
}
