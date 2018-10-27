using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynUtil.FileSystem
{
    public class LogInfo
    {
        private string rootFolder;
        private string logFolder;
        private string logFileName;
        private string appendDateTimeFormat; //DateTime.ToString('<FORMAT>') format for the ToString
        private bool appendDateTime; //Should the LogFileName append the DateTime to the end of the FileName using the AppendDateTimeFormat
        private bool isDebug;
        private bool pathValidated;

        public bool AppendDateTime
        {
            get { return appendDateTime; }
            set { appendDateTime = value; }
        }
        public string AppendDateTimeFormat
        {
            get { return appendDateTimeFormat; }
            set { appendDateTimeFormat = value; }
        }
        public bool IsDebug
        {
            get { return isDebug; }
            set { isDebug = value; }
        }
        /// <summary>
        /// If you set the "AppendDateTime" bool to true the get{} will compute the FileName using the "AppendDateTimeFormat" you specify
        /// </summary>
        public string LogFileName
        {
            get
            {
                if (String.IsNullOrEmpty(logFileName))
                    throw new Exception("LogInfo Filename has not been set. Must set a LogFileName.");

                if (appendDateTime)
                {
                    string computedLogFileName = String.Empty;
                    string fileNamePart = String.Empty;
                    string fileExtension = String.Empty;

                    if (logFileName.Contains('.'))
                    {
                        int seperatorIndex = logFileName.LastIndexOf('.');
                        fileNamePart = logFileName.Substring(0, seperatorIndex);
                        fileExtension = logFileName.Substring(seperatorIndex); //Keep the dot so that it is easier to reconstruct the path
                    }
                    else
                    {
                        fileNamePart = logFileName;
                    }

                    computedLogFileName = fileNamePart + " " + DateTime.Now.ToString(appendDateTimeFormat) + fileExtension;

                    return computedLogFileName;
                }
                else
                {
                    return logFileName;
                }
            }
            set { logFileName = value; }
        }
        public string LogFolder
        {
            get { return logFolder; }
            set { logFolder = value; }
        }
        public string RootFolder
        {
            get { return rootFolder; }
            set { rootFolder = value; }
        }
        public string FullPath
        {
            get
            {
                string path = Path.Combine(rootFolder, logFolder);
                return Path.Combine(path, LogFileName);
            }
        }
        public bool PathValidated
        {
            get { return pathValidated; }
            set { pathValidated = value; }
        }

        public LogInfo()
        {
            rootFolder = logFolder = logFileName = appendDateTimeFormat = String.Empty;
            isDebug = appendDateTime = pathValidated = false;
        }
    }
}
