using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using NewLife.Log;

namespace ThreadMan
{
    public class Program
    {
        static void Main(string[] args)
        {
            XTrace.UseConsole();
            SMTS.ServiceMain();

            Console.ReadKey();
        }
    }

}
