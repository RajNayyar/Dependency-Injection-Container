using System;
using System.Collections.Generic;
using System.Text;

namespace DIContainers
{
    public static class DIContainerExtensions
    {
        public static DIContainer Register<TContract, TImpl>(this DIContainer container)
            where TImpl : TContract
        {
            container.Register(typeof(TContract), typeof(TImpl));
            return container;
        }

        public static T Build<T>(this DIContainer container)
            where T : class
        {
            return container.Build(typeof(T)) as T;
        }

    }
}
