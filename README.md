# XmlLogLibrary

## ABOUT

一个简单易用的log日志记录库。需要.net core 3.1及以上版本使用。

## 使用

```csharp
using XmlLogLibrary

namespace YourProject{
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
