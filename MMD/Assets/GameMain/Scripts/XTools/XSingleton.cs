using System;
using System.Threading;

namespace Penny
{
    public class XSingleton<T> where T  : class, new()
    {
        protected static T _instance = default(T);
        private static object _lock = new object();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    object obj;
                    Monitor.Enter(obj = _lock);//加锁防止多线程创建单例
                    try
                    {
                        if (_instance == null)
                        {
                            _instance = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));//创建单例的实例
                        }
                    }
                    finally
                    {
                        Monitor.Exit(obj);
                    }
                }
                return XSingleton<T>._instance;
            }
        }

        public static void Destroy()
        {
            _instance = null;
        }
    }
}
