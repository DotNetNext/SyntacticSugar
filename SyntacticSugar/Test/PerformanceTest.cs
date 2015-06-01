using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：程序性能测试类
    /// ** 创始时间：2015-5-30
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明：http://www.cnblogs.com/sunkaixuan/p/4540840.html
    /// </summary>
    public class PerformanceTest
    {
        private DateTime BeginTime;
        private DateTime EndTime;
        private ParamsModel Params;

        /// <summary>
        ///设置执行次数(默认:1)
        /// </summary>
        public void SetCount(int count)
        {
            Params.RunCount = count;
        }
        /// <summary>
        /// 设置线程模式(默认:false)
        /// </summary>
        /// <param name="isMul">true为多线程</param>
        public void SetIsMultithread(bool isMul)
        {
            Params.IsMultithread = isMul;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PerformanceTest()
        {
            Params = new ParamsModel()
            {
                RunCount = 1
            };
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="action"></param>
        public void Execute(Action<int> action, Action<string> rollBack)
        {
            List<Thread> arr = new List<Thread>();
            BeginTime = DateTime.Now;
            for (int i = 0; i < Params.RunCount; i++)
            {
                if (Params.IsMultithread)
                {
                    var thread = new Thread(new System.Threading.ThreadStart(() =>
                    {
                        action(i);
                    }));
                    thread.Start();
                    arr.Add(thread);
                }
                else
                {
                    action(i);
                }
            }
            if (Params.IsMultithread)
            {
                foreach (Thread t in arr)
                {
                    while (t.IsAlive)
                    {
                        Thread.Sleep(10);
                    }
                }

            }
            rollBack(GetResult());
        }

        public string GetResult()
        {
            EndTime = DateTime.Now;
            string totalTime = ((EndTime - BeginTime).TotalMilliseconds / 1000.0).ToString("n5");
            string reval = string.Format("总共执行时间：{0}秒", totalTime);
            Console.Write(reval);
            return reval;
        }

        private class ParamsModel
        {
            public int RunCount { get; set; }
            public bool IsMultithread { get; set; }
        }
    }
}
