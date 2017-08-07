using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exercises
{
    class Program
    {
        static void Main(string[] args)
        {
            //Listing1();
            //Listing2();
            //Listing3();
            //Listing4();
            //Listing5();
            //Listing6();
            Listing7();
            
            //Console.WriteLine("Press any button to continue...");
            //Console.ReadLine();
        }

        private static void Listing1()
        {
            var thread = new System.Threading.Thread(new System.Threading.ThreadStart(Chapter1.Threads.ThreadMethod));
            thread.Start();

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("Main Thread: Do some work!");
                System.Threading.Thread.Sleep(0); //warn windows that this thread is finished, switching to another thread
            }

            thread.Join();           
        }
        
        /// <summary>
        /// Background thread running
        /// </summary>
        private static void Listing2()
        {
            var thread = new System.Threading.Thread(new System.Threading.ThreadStart(Chapter1.Threads.ThreadMethod1));
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// Parameterized Threads
        /// </summary>
        private static void Listing3()
        {
            var thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Chapter1.Threads.ThreadMethod2));

            var obj = 5;

            thread.Start(obj);
            thread.Join();
        }

        /// <summary>
        /// A better way to stop a thread using a shared variable 
        /// </summary>
        private static void Listing4()
        {

            var stopped = false;

            var thread = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                while (!stopped)
                {
                    Console.WriteLine("Thread running...");
                    Thread.Sleep(500);
                }

            }));

            thread.Start();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            stopped = true;

            thread.Join();
        }

                
        [ThreadStatic]
        public static int _fieldListing5;
        /// <summary>
        /// Using the ThreadStaticAttribute [ThreadStatic]
        /// Each thread gets its own copy of a field, max value of _field becomes 10. 
        /// Without the static attribute, max value becomes 20 because both threads access the same value.
        /// </summary>
        private static void Listing5()
        {

            new System.Threading.Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    _fieldListing5++;
                    Console.WriteLine("Thread A: {0}", _fieldListing5);
                }
            }).Start();

            new System.Threading.Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    _fieldListing5++;
                    Console.WriteLine("Thread B: {0}", _fieldListing5);
                }
            }).Start();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        public static ThreadLocal<int> _fieldListing6 = new ThreadLocal<int>(() =>
        {
            return Thread.CurrentThread.ManagedThreadId;
        });
        /// <summary>
        /// Using ThreadLocal to use local data for each thread
        /// </summary>
        private static void Listing6()
        {
            new System.Threading.Thread(() =>
            {
                for (int i = 0; i < _fieldListing6.Value; i++)
                {
                    Console.WriteLine("Thread A: {0}", i);
                }
            }).Start();

            new System.Threading.Thread(() =>
            {
                for (int i = 0; i < _fieldListing6.Value; i++)
                {
                    Console.WriteLine("Thread B: {0}", i);
                }
            }).Start();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        /// <summary>
        /// Using ThreadPool to reuse threads created, avoid the cost of create thread all the time
        /// </summary>
        private static void Listing7()
        {
            ThreadPool.QueueUserWorkItem((s) =>
            {
                Console.WriteLine("Working on a thread from ThreadPool");
            });

            Console.ReadLine();
        }
    }
}