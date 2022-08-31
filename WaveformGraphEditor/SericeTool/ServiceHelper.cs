namespace SericeTool
{
    public class ServiceHelper
    {
        IServiceProvider services;
        private static ServiceHelper instance = null;
        private static readonly object locker = new object();
        private ServiceHelper()
        {

        }

        public static ServiceHelper GetInstance(IServiceProvider serviceProvider)
        {
            var s= GetInstance();
            s.services = serviceProvider;
            return s;
        }

        public static ServiceHelper GetInstance()

        {

            if (instance == null)
            {

                lock (locker)

                {

                    if (instance == null)

                    {

                         instance = new ServiceHelper();

                    }

                }

                return instance;
            }
            return instance;
        }

        public IServiceProvider GetServiceProvider()
        {
            return services;
        }
    }
}