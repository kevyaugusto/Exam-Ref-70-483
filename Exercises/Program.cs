using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            //Listing7();
            //Listing8();
            //Listing9();
            //Listing10();
            //Listing11();
            //Listing12();
            //Listing13();
            //Listing14();
            //Listing15();
            //Listing16();
            //Listing17();
            //Listing18();
            //Listing19();
            //Listing22();
            //Listing23();
            //Listing24();
            //Listing25();
            //Listing26();
            Listing27();

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

        /// <summary>
        /// Starting a new Task
        /// </summary>
        private static void Listing8()
        {
            var task = Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine("*");
                }
            });

            task.Wait();
        }

        /// <summary>
        /// Using a Task that returns a value
        /// </summary>
        private static void Listing9()
        {
            var task = Task<int>.Run(() =>
            {
                return int.MaxValue;
            });

            Console.WriteLine(task.Result);
            Console.ReadKey();
        }

        /// <summary>
        /// Adding a continuation to the task
        /// </summary>
        private static void Listing10()
        {
            var task = Task<int>.Run(() =>
            {
                return int.MaxValue;
            }).ContinueWith((i) =>
            {
                return i.Result / 10000;
            });

            Console.WriteLine(task.Result);
            Console.ReadKey();
        }

        /// <summary>
        /// Scheduling different continuation tasks
        /// </summary>
        private static void Listing11()
        {
            var task = Task<int>.Run(() =>
            {
                return int.MaxValue;
            });

            task.ContinueWith((i) =>
            {
                Console.WriteLine("Canceled");
            }, TaskContinuationOptions.OnlyOnCanceled);

            task.ContinueWith((i) =>
            {
                Console.WriteLine("Faulted");
            }, TaskContinuationOptions.OnlyOnFaulted);

            var completedTask = task.ContinueWith((i) =>
            {
                Console.WriteLine("Completed");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            completedTask.Wait();

            Console.WriteLine(task.Result);
            Console.ReadKey();
        }

        /// <summary>
        /// Attaching child tasks a parent task
        /// </summary>
        private static void Listing12()
        {
            var parent = Task.Run(() =>
            {
                var results = new int[3];

                new Task(() => results[0] = 0, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = 5, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = 10, TaskCreationOptions.AttachedToParent).Start();

                return results;
            });

            var finalTask = parent.ContinueWith(parentTask =>
            {
                foreach (int i in parentTask.Result)
                {
                    Console.WriteLine(i);
                }
            });

            finalTask.Wait();
            Console.ReadKey();
        }

        /// <summary>
        /// Using a TaskFactory
        /// </summary>
        private static void Listing13()
        {
            var parent = Task.Run(() =>
            {
                var results = new int[3];

                var taskFactory = new TaskFactory(TaskCreationOptions.AttachedToParent, TaskContinuationOptions.ExecuteSynchronously);

                taskFactory.StartNew(() => results[0] = 0);
                taskFactory.StartNew(() => results[1] = 5);
                taskFactory.StartNew(() => results[2] = 10);

                return results;
            });

            var finalTask = parent.ContinueWith(parentTask =>
            {
                foreach (int i in parentTask.Result)
                {
                    Console.WriteLine(i);
                }
            });

            finalTask.Wait();
            Console.ReadKey();
        }

        /// <summary>
        /// Using Task.WaitAll finish.
        /// All tasks are executed simultaneously and takes 1s instead of 3s
        /// </summary>
        private static void Listing14()
        {
            var tasks = new Task[3];

            tasks[0] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine(1);
                return 1;
            });

            tasks[1] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine(2);
                return 2;
            });

            tasks[2] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine(3);
                return 3;
            });

            Task.WaitAll(tasks);
            Console.ReadKey();
        }

        /// <summary>
        /// Using Task.WaitAny finishes.
        /// </summary>
        private static void Listing15()
        {
            var tasks = new Task<int>[3];

            tasks[0] = Task.Run(() =>
            {
                Thread.Sleep(2000);
                return 1;
            });

            tasks[1] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                return 2;
            });

            tasks[2] = Task.Run(() =>
            {
                Thread.Sleep(3000);
                return 3;
            });

            while (tasks.Length > 0)
            {
                var i = Task.WaitAny(tasks);
                var completedTask = tasks[i];

                Console.WriteLine(completedTask.Result);

                var temp = tasks.ToList();
                temp.RemoveAt(i);
                tasks = temp.ToArray();

            }

            Console.ReadKey();
        }

        /// <summary>
        /// Using Parallel.For and Parallel.ForEach
        /// </summary>
        private static void Listing16()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Parallel.For(0, 10, (i) =>
            {
                Thread.Sleep(500);
            });

            Console.WriteLine("Total execution Parallel.For: {0} ms", stopWatch.ElapsedMilliseconds);

            stopWatch.Restart();

            var numbers = Enumerable.Range(0, 10);
            Parallel.ForEach(numbers, (i) =>
            {
                Thread.Sleep(500);
            });

            Console.WriteLine("Total execution Parallel.ForEach: {0} ms", stopWatch.ElapsedMilliseconds);

            Console.ReadKey();
        }

        /// <summary>
        /// Using Parallel.Break
        /// </summary>
        private static void Listing17()
        {
            ParallelLoopResult loopResult = Parallel.For(0, 1000, (int i, ParallelLoopState loopState) =>
            {
                if (i == 500)
                {
                    Console.WriteLine("Breaking state");
                    loopState.Break();
                }
                return;
            });

            Console.WriteLine(loopResult.IsCompleted);
            Console.WriteLine(loopResult.LowestBreakIteration);
            Console.ReadKey();
        }

        /// <summary>
        /// Async and Await
        /// </summary>
        private static void Listing18()
        {
            var result = DownloadContent().Result;

            Console.WriteLine(result);

            Console.ReadKey();
        }

        private static async Task<string> DownloadContent()
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var result = await httpClient.GetStringAsync("http://www.microsoft.com");
                return result;
            }
        }

        /// <summary>
        /// Scalability versus responsiveness
        /// </summary>
        private static void Listing19()
        {
            var millisecondsTimeout = 10000;
            var sw = Stopwatch.StartNew();

            var result = SleepAsyncA(millisecondsTimeout);
            Console.WriteLine("Time to finish SleepAsyncA in milliseconds: {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            var result2 = SleepAsyncB(millisecondsTimeout);

            Console.WriteLine("Time to finish SleepAsyncB in milliseconds: {0}", sw.ElapsedMilliseconds);
            Console.ReadKey();
        }

        /// <summary>
        /// This method uses a thread from the ThreadPool while sleeping
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        private static Task SleepAsyncA(int millisecondsTimeout)
        {
            return Task.Run(() => Thread.Sleep(millisecondsTimeout));
        }

        /// <summary>
        /// This method does not occupy a thread while waiting for the timer to run giving SCALABILITY
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        private static Task SleepAsyncB(int millisecondsTimeout)
        {
            TaskCompletionSource<bool> taskCompSour = null;

            var timer = new Timer(delegate { taskCompSour.SetResult(true); }, null, -1, -1);

            taskCompSour = new TaskCompletionSource<bool>(timer);
            timer.Change(millisecondsTimeout, -1);

            return taskCompSour.Task;
        }

        /// <summary>
        /// Using AsParallel
        /// </summary>
        private static void Listing22()
        {
            var numbers = Enumerable.Range(0, 1000000);

            var parallelResult = numbers.AsParallel().Where(n => n % 2 == 0).ToArray();

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        /// <summary>
        /// Unordered parallel query
        /// </summary>
        private static void Listing23()
        {
            var numbers = Enumerable.Range(0, 20);

            var parallelResult = numbers.AsParallel().Where(n => n % 2 == 0).ToArray();

            foreach (var number in parallelResult)
            {
                Console.WriteLine(number);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Ordered parallel query
        /// </summary>
        private static void Listing24()
        {
            var numbers = Enumerable.Range(0, 20);

            var parallelResult = numbers.AsParallel().AsOrdered()
                .Where(n => n % 2 == 0).ToArray();

            foreach (var number in parallelResult)
            {
                Console.WriteLine(number);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Making a parallel query sequencial
        /// </summary>
        private static void Listing25()
        {
            var numbers = Enumerable.Range(0, 20);

            var parallelResult = numbers.AsParallel().AsOrdered()
                .Where(n => n % 2 == 0).AsSequential();

            foreach (var number in parallelResult.Take(5))
            {
                Console.WriteLine(number);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Using ForAll method to iterate in a parallel way
        /// </summary>
        private static void Listing26()
        {
            var numbers = Enumerable.Range(0, 20);

            var parallelResult = numbers.AsParallel()
                .Where(n => n % 2 == 0);

            parallelResult.ForAll(number => Console.WriteLine(number));

            Console.ReadKey();
        }

        /// <summary>
        /// Catching Aggregate Exception
        /// </summary>
        private static void Listing27()
        {
            var numbers = Enumerable.Range(0, 20);

            try
            {
                var parallelResult = numbers.AsParallel()
                    .Where(n => IsEven(n));

                parallelResult.ForAll(number => Console.WriteLine(number));
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("There were {0} exceptions", ae.InnerExceptions.Count);
            }

            Console.ReadKey();
        }

        private static bool IsEven(int number)
        {
            if (number % 10 == 0)
                throw new ArgumentException("forced exception");

            return number % 2 == 0;
        }
    }
}