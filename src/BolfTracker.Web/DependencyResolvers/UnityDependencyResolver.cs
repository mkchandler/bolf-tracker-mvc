using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Microsoft.Practices.Unity;

namespace BolfTracker.Web
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer _unityContainer;

        public UnityDependencyResolver(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public object GetService(Type serviceType)
        {
            if (!_unityContainer.IsRegistered(serviceType))
            {
                if (serviceType.IsAbstract || serviceType.IsInterface)
                {
                    return null;
                }
            }

            return _unityContainer.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _unityContainer.ResolveAll(serviceType);
        }
    }
}