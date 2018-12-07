using System;
using System.Collections.Generic;
using System.Text;

namespace DIContainers
{
    public static class DIContainerExtensions
    {
        public static DIContainer Register<TContract, TImpl>(this DIContainer container, string name = null)
            where TImpl : TContract
        {
            container.Register(typeof(TContract), typeof(TImpl), name);
            return container;
        }

        public static T Build<T>(this DIContainer container, string name = null)
            where T : class
        {
            return container.Build(typeof(T), name) as T;
        }

    }
}
