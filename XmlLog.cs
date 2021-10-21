using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace XmlLogLibrary
{
    public class XmlLog
    {
        private readonly static object lockObj = new object();
        private static XmlLog instance = null;

        public static XmlLog Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new XmlLog();
                        }
                    }
                }
                return instance;
            }
        }

        private string logDirectoryPath = Directory.GetCurrentDirectory() + "\\log";
        private string logPath = Directory.GetCurrentDirectory() + "\\log" + "\\log.xml";
        private int logFileLength = 1024 * 1024 * 5;
        private XDocument logFile;

        /// <summary>
        /// 初始化Log系统，如果没有log文件就建立，log文件默认在程序目录的log目录下。 log单文件默认最高大小为1024*1024*5 = 5Mb
        /// </summary>
        public void InitLog()
        {
            initLog();
        }

        /// <summary>
        /// 初始化Log系统，如果没有log文件就建立
        /// </summary>
        /// <param name="logDirectoryPath">log文件的存储目录</param>
        /// <param name="logName">log文件的名字，不带扩展名</param>
        /// <param name="length">log文件大小，超过该大小则建立新文件，源文件重命名后备份</param>
        public void InitLog(string logDirectory, string logName, int length)
        {
            logDirectoryPath = logDirectory;
            logPath = logDirectory + "\\" + logName + ".xml";
            logFileLength = length;
            initLog();
        }

        private void initLog()
        {
            if (!Directory.Exists(logDirectoryPath))
            {
                Directory.CreateDirectory(logDirectoryPath);
            }
            if (File.Exists(logPath))
            {
                logFile = XDocument.Load(logPath);
                return;
            }
            else
            {
                string initLogString = Resource.initLog;
                FileStream newLogFile = File.Create(logPath);
                newLogFile.Write(System.Text.Encoding.UTF8.GetBytes(initLogString));
                newLogFile.Close();
                logFile = XDocument.Load(logPath);
            }
        }

        private void createNewLog()
        {
            FileInfo fileInfo = new FileInfo(logPath);
            if (fileInfo.Length > logFileLength)
            {
                try
                {
                    File.Move(logPath, logDirectoryPath + "\\log_" + DateTime.Now.ToLocalTime().ToString() + ".xml");
                    initLog();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 增加一条log
        /// </summary>
        /// <param name="data">log内容</param>
        /// <param name="type">log的类型</param>
        /// <param name="kind">log级别</param>
        public void AddLog(object data, string type, string kind)
        {
            try
            {
                initLog();
                createNewLog();
                XElement logRoot = logFile.Element("root");
                string datetime = DateTime.Now.ToLocalTime().ToString();
                logRoot.AddFirst(new XElement("log", data, new XAttribute("type", type), new XAttribute("kind", kind), new XAttribute("datetime", datetime)));
                logFile.Save(logPath);
            }
            catch
            {
            }
        }

        public List<LogBase> LogList
        {
            get
            {
                XDocument logFile = XDocument.Load(logPath);
                XElement logRoot = logFile.Element("root");
                List<LogBase> list = new List<LogBase>();
                if (logRoot.HasElements)
                {
                    logRoot.Elements().ToList().ForEach(v =>
                    {
                        LogBase logBase = new LogBase();
                        logBase.data = v.Value;
                        logBase.kind = v.Attribute("kind").Value;
                        logBase.type = v.Attribute("type").Value;
                        logBase.datetime = v.Attribute("datetime").Value;
                        list.Add(logBase);
                    });
                }
                return list;
            }
        }
    }
}