using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class Program
    {
        //http://www.cnblogs.com/sheng-jie/p/6471986.html
        //        3.3. 什么是前台线程
        //        默认情况下，使用Thread.Start()方法创建的线程都是前台线程。前台线程能阻止应用程序的终结，只有所有的前台线程执行完毕，CLR才能关闭应用程序（即卸载承载的应用程序域）。前台线程也属于工作者线程。

        //3.4. 什么是后台线程
        //        后台线程不会影响应用程序的终结，当所有前台线程执行完毕后，后台线程无论是否执行完毕，都会被终结。一般后台线程用来做些无关紧要的任务（比如邮箱每隔一段时间就去检查下邮件，天气应用每隔一段时间去更新天气）。后台线程也属于工作者线程。

        static void Main(string[] args)
        {
           

            //C# async关键字用来指定某个方法、Lambda表达式或匿名方法自动以异步的方式来调用。
            Console.WriteLine("主线程启动，当前线程为：" + Thread.CurrentThread.ManagedThreadId);
            var task = GetLengthAsync();

            Console.WriteLine("回到主线程，当前线程为：" + Thread.CurrentThread.ManagedThreadId);

            Console.WriteLine("线程【" + Thread.CurrentThread.ManagedThreadId + "】睡眠5s:");
            Thread.Sleep(5000);  //将主线程睡眠5s

            var timer = new Stopwatch();
            timer.Start();      //开始计算时间

            Console.WriteLine("task的返回值是" + task.Result);

            timer.Stop();    //结束点，另外StopWatch还有Reset方法，可以重置。
            Console.WriteLine("等待了："+timer.Elapsed.TotalSeconds+"秒");//显示时间

            Console.WriteLine("主线程结束，当前线程为："+Thread.CurrentThread.ManagedThreadId);


            //Console.WriteLine("主线程开始！");

            //ForeBackGround();
            //ThreadPoolTest();
             Console.ReadKey();
        }

        private static async Task<int> GetLengthAsync()
        {
            Console.WriteLine("GetLengthAsync()开始执行，当前线程为：" + Thread.CurrentThread.ManagedThreadId);

            var str = await GetStringAsync();

            Console.WriteLine("GetLengthAsync()执行完毕，当前线程为：" + Thread.CurrentThread.ManagedThreadId);

            return str.Length;
        }

        private static Task<string> GetStringAsync()
        {
            Console.WriteLine("GetStringAsync()开始执行，当前线程为：" + Thread.CurrentThread.ManagedThreadId);
            return Task.Run(() =>
{
    Console.WriteLine("异步任务开始执行，当前线程为：" + Thread.CurrentThread.ManagedThreadId);

    Console.WriteLine("线程【" + Thread.CurrentThread.ManagedThreadId + "】睡眠10s");
    Thread.Sleep(10000);   //将异步任务线程睡眠10s 

    return "GetStringAsync()执行完毕";

});

        }

        public static void TaskDemo2()
        {
            Console.WriteLine("主线程ID：" + Thread.CurrentThread.ManagedThreadId);
            Task.Run(() => Console.WriteLine("Task对应线程ID：" + Thread.CurrentThread.ManagedThreadId));
            Console.ReadLine();
        }

        public static void TaskDemo()
        {
            Console.WriteLine("主线程ID：" + Thread.CurrentThread.ManagedThreadId);

            Task.Factory.StartNew(() => Console.WriteLine("Task对应线程ID：" + Thread.CurrentThread.ManagedThreadId));
            Console.ReadKey();
        }

        /// <summary>
        /// ThreadPool（线程池）
        /// </summary>
        public static void ThreadPoolTest()
        {
            WaitCallback workItem = state => Console.WriteLine("当前线程Id为：" + Thread.CurrentThread.ManagedThreadId);

            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(workItem);
            }

        }

        /// <summary>
        /// 前台线程、后台线程
        /// </summary>
        private static void ForeBackGround()
        {
            //创建前台工作线程
            Thread t1 = new Thread(Task1);
            t1.Start();

            //创建后台工作线程
            Thread t2 = new Thread(new ParameterizedThreadStart(Task2));
            t2.IsBackground = true; //设置为后台线程
            t2.Start("传参");
        }

        private static void Task1()
        {
            Thread.Sleep(1000); //模拟耗时操作，睡眠1S
            Console.WriteLine("前台线程被调用！");
        }

        private static void Task2(object data)
        {
            Thread.Sleep(2000);   //模拟耗时操作，睡眠2S
            Console.WriteLine("后台线程被调用！" + data);
        }

    }
}
