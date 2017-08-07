using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exercises.Chapter1
{
    class Threads
    {
        public static void ThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("ThreadProcessing: {0}", i);
                Thread.Sleep(0);
            }
        }

        public static void ThreadMethod1()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("ThreadProcessing: {0}", i);
                Thread.Sleep(1000);
            }
        }

        public static void ThreadMethod2(object arg1)
        {
            for (int i = 0; i < (int)arg1; i++)
            {
                Console.WriteLine("ThreadProcessing: {0}", i);
                Thread.Sleep(500);
            }
        }
    }
}
