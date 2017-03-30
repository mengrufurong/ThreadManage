using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Xml;

namespace ThreadMan
{
    [XmlConfigFile(@"ThreadMan.xml", 15000)]
    public class ThreadInfoDto:XmlConfig<ThreadInfoDto>
    {
        public ThreadInfoDto()
        {
            TimeSpan = 60;
            new List<ProcessInfo>();
            new List<ServiceInfo>();

        }

        //控制台程序运行时间间隔,单位（秒）
        public int TimeSpan { get; set; }
        

        public List<ProcessInfo> ProcessInfos { get; set; }
        public List<ServiceInfo> ServiceInfos { get; set; }
    }
    
    public class ProcessInfo
    {
        public string PathFile { get; set; }
        public string ProcessType { get; set; }
        public string ProcessName { get; set; }
        //进程状态，0表示未启动，1表示启动
        public bool Status { get; set; }
        //进程停止次数
        public int Count { get; set; }

        public ProcessInfo()
        {
            Count = 0;
        }
    }
    public class ServiceInfo
    {
        public string ServiceName { get; set; }
        public bool Staus { get; set; }
    }
}
