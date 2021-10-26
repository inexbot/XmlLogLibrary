# XmlLogLibrary

## ABOUT

一个简单易用的 log 日志记录库。需要.net core 3.1 及以上版本使用。

推荐使用 Nuget 安装库并使用[https://www.nuget.org/packages/XmlLogLibrary](https://www.nuget.org/packages/XmlLogLibrary)。

## API

```csharp
public class LogBase
{
    public object data; //日志内容
    public string type; //日志类型
    public string kind; //日志等级
    public string datetime; //日志时间
}
```

```csharp
public class XmlLog
{
	public static XmlLog Instance;//实例
	public void InitLog();//使用默认参数初始化Log，仅需初始化一次
	public void InitLog(string directory,string logName,int logLength);//使用自定义log的位置、名称和大小来初始化Log
	public void AddLog(string data,string type,string kind);//写日志，日志内容、日志类型、日志等级
	public List<LogBase> LogList;//日志列表
}
```

## 使用

```csharp
using XmlLogLibrary

namespace YourProject
{
	class Main{
		private XmlLog log = XmlLog.Instance;
		private Main(){
			log.InitLog(); //使用默认参数初始化Log，仅需初始化一次;
			//Log.InitLog(Directory.GetCurrentDirectory()+"\\log","mylog",1024*1024); 自定义log位置、名称和大小
			log.AddLog("软件打开", "local", "1");
			List<LogBase> logs = log.LogList;
			Console.WriteLine(logs[0].data);
		}
	}
}
```
