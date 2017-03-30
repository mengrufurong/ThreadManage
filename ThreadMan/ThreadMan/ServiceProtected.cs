using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using NewLife.Log;

namespace ThreadMan
{
    public class ServiceProtected
    {
        
        public ServiceProtected()
        {
            //ServiceController sc = new ServiceController("Server");
            //if (sc.Status.Equals(ServiceControllerStatus.Running))
            //{
            //    sc.Stop();
            //    sc.Refresh();
            //}
            //if ((sc.Status.Equals(ServiceControllerStatus.Stopped)) || (sc.Status.Equals(ServiceControllerStatus.StopPending)))
            //{
            //    sc.Start();
            //    sc.Refresh();
            //}
        }
        //查询当前系统中的所有windows服务
        public static void WatchService()
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
               
                foreach (ServiceController service in services)
                {

                    XTrace.WriteLine(service.ServiceName);  
                }

                XTrace.WriteLine("服务查询成功！");
            }
            catch
            {
                XTrace.WriteLine("服务查询失败！");
            }
        }

        //判断XML中的windows服务是否启动
        public static void ServiceRun()
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                 List<ServiceInfo> removeList=new List<ServiceInfo>();
                List<ServiceInfo> addList = new List<ServiceInfo>();
                foreach (var serviceInfo in ThreadInfoDto.Current.ServiceInfos)
                {
                    foreach (ServiceController service in services)
                    {
                        if (service.ServiceName == serviceInfo.ServiceName)
                        {
                            if (service.Status != ServiceControllerStatus.Running)
                            {
                                StartService(serviceInfo.ServiceName);
                            }
                            else
                            {
                                XTrace.WriteLine(serviceInfo.ServiceName+"服务已经启动！");
                            }
                        }
                    }
                    removeList.Add(serviceInfo);
                    serviceInfo.Staus = true;
                    addList.Add(serviceInfo);
                }
                #region 删除已经修改之前的，添加修改后的ServiceInfos对象
                if (removeList.Any())
                {
                    foreach (var rem in removeList)
                    {
                        ThreadInfoDto.Current.ServiceInfos.Remove(rem);
                    }
                }
                if (addList.Any())
                {
                    foreach (var addl in addList)
                    {
                        ThreadInfoDto.Current.ServiceInfos.Add(addl);
                    }
                }
                ThreadInfoDto.Current.Save(); 
                #endregion
            }
            catch
            {
                XTrace.WriteLine("windows服务运行失败！");
            }
        }
        //启动服务
        public static void StartService(string serviceName)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                    {
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
                        XTrace.WriteLine(serviceName + "服务启动成功！");
                    }
                }
            }
            catch
            {
                XTrace.WriteLine(serviceName+"服务运行失败！");
            }
        }
    }
}
