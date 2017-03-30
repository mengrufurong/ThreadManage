//   SOLUTION  ： ThreadManage
//   PROJECT     ： ThreadMan
//   FILENAME   ： ProcessProtected.cs
//   AUTHOR     ： 陈奇
//   CREATE TIME： 2017-03-28
//   UPDATE TIME：2017-03-28 15:41
//   COPYRIGHT  ： 版权所有 (C) 安徽斯玛特物联网科技有限公司 http://www.smartiot.cc/ 2011~2017

#region Namespace

using System;
using System.Diagnostics;
using System.Threading;
using NewLife.Log;

#endregion

namespace ThreadMan
{
    public class ProcessProtected
    {
        public ProcessProtected()
        {
        }


        public static void Watch()
        {
            var IsRun = true;
            var tempPro = ThreadInfoDto.Current.ProcessInfos;
            foreach (var s in tempPro)
            {
                var proName = s.ProcessName.Remove(s.ProcessName.LastIndexOf("."));
                var proc = Process.GetProcessesByName(proName);
                if (proc.Length > 0)
                {
                    foreach (var process in proc)
                        if (process.MainModule.FileName == s.PathFile)
                            IsRun = false;
                    if (IsRun)
                    {
                        StartProcess(s);
                        s.Status = true;
                    }
                    else
                    {
                        XTrace.WriteLine(s.ProcessName + "进程已经启动！");
                    }
                }
                else
                {
                    StartProcess(s);
                    s.Status = true;
                }
            } 
        }

        public static void StartProcess(ProcessInfo proInfoTemp)
        {
            var proInfo = new ProcessStartInfo(proInfoTemp.PathFile);
            try
            {
                Process.Start(proInfo);
                XTrace.WriteLine("正在启动" + proInfoTemp.ProcessName + "中。。。。");
                proInfoTemp.Count++;
                XTrace.WriteLine(proInfoTemp.ProcessName + "进程启动成功！");
            }
            catch (Exception ex)
            {
                XTrace.WriteLine(proInfoTemp.ProcessName + "进程启动失败！");
                XTrace.WriteException(ex);
            }
            proInfoTemp.Status = true;
            ThreadInfoDto.Current.Save();
            XTrace.WriteLine("进程启动完毕！");
        }

        //关闭进程
        public static void CloseProcess()
        {
            var tempPro = ThreadInfoDto.Current.ProcessInfos;
            foreach (var s in tempPro)
            {
                var processNames = new string[2];
                processNames = s.ProcessName.Split(".");
                var proc = Process.GetProcessesByName(processNames[0]);
                for (var i = 0; i < proc.Length; i++)
                    proc[i].Kill();
                XTrace.WriteLine("进程正在关闭中。。。。。。");
            }
            Thread.Sleep(2000);
            XTrace.WriteLine("关闭进程成功！");
        }
    }
}