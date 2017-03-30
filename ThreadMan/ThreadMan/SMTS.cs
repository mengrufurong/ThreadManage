//   SOLUTION  ： ThreadManage
//   PROJECT     ： ThreadMan
//   FILENAME   ： SMTS.cs
//   AUTHOR     ： 陈奇
//   CREATE TIME： 2017-03-28
//   UPDATE TIME：2017-03-28 15:35
//   COPYRIGHT  ： 版权所有 (C) 安徽斯玛特物联网科技有限公司 http://www.smartiot.cc/ 2011~2017

using System.Threading;
using NewLife.Agent;
using NewLife.Log;

namespace ThreadMan
{
    public class SMTS : AgentServiceBase<SMTS>
    {
        private Timer _timer;

        public override void StartWork()
        {
            if (_timer == null)
            {
                _timer = new Timer(obj =>
                {
                    ProcessProtected.Watch();
                    ServiceProtected.ServiceRun();
                }, null, 1000, ThreadInfoDto.Current.TimeSpan*1000);
            }

            base.StartWork();
            XTrace.WriteLine("守护服务已启动");
        }

        public override void StopWork()
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            base.StopWork();
            XTrace.WriteLine("守护服务已停止");
        }
    }
}