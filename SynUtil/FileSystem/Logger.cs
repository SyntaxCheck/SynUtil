using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SynUtil.FileSystem
{
    public class Logger
    {
        public static void Write(string logPath, string logMessage, int retryCount = 0)
        {
            int attempts = 0;

            while (attempts <= retryCount)
            {
                attempts++;
                try
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

                    return;
                }
                catch (Exception ex)
                {
                    if (attempts >= retryCount)
                        throw ex;
                    else
                        Thread.Sleep(250);
                }
            }
        }
        public static void Write(LogInfo logInfo, string logMessage)
        {
            //If we have not validated the path yet then call the ValidatePath. This will build the folder structure if it does not exist
            if (!logInfo.PathValidated)
                ValidateLogPath(ref logInfo);

            Write(logInfo.FullPath, logMessage, logInfo.RetryCount);
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
        public static void WriteDbg(string logPath, string logMessage, bool isDebug = true, int retryCount = 0)
        {
            if (isDebug)
            {
                Write(logPath, logMessage, retryCount);
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
            string program = "SysUtil";

            if (String.IsNullOrEmpty(logInfo.RootFolder) || String.IsNullOrEmpty(logInfo.LogFolder))
            {
                if (!EventLog.SourceExists(program))
                    EventLog.CreateEventSource(program, "Application");

                string message = "Log Validate Path: RootFolder and/or LogFolder not provided";

                EventLog.WriteEntry(program, message, EventLogEntryType.Error);

                throw new Exception(message);
            }
            else
            {
                //Verify that the root folder exists
                if (!Directory.Exists(logInfo.RootFolder))
                {
                    if (!EventLog.SourceExists(program))
                        EventLog.CreateEventSource(program, "Application");

                    string message = "Log Validate Path: RootFolder (" + logInfo.RootFolder + ") does not exist or we do not have permissions to it";

                    EventLog.WriteEntry(program, message, EventLogEntryType.Error);

                    throw new Exception(message);
                }
                else
                {
                    //Build the log subfolder if it does not exist
                    if (!Directory.Exists(Path.Combine(logInfo.RootFolder, logInfo.LogFolder)))
                    {
                        try
                        {
                            Directory.CreateDirectory(Path.Combine(logInfo.RootFolder, logInfo.LogFolder));
                        }
                        catch (Exception ex)
                        {
                            if (!EventLog.SourceExists(program))
                                EventLog.CreateEventSource(program, "Application");

                            string message = "Failed to build directory tree: " + Path.Combine(logInfo.RootFolder, logInfo.LogFolder);

                            EventLog.WriteEntry(program, message, EventLogEntryType.Error);

                            throw new Exception(message, ex);
                        }
                    }

                    logInfo.PathValidated = true;
                }
            }
        }
    }
}
