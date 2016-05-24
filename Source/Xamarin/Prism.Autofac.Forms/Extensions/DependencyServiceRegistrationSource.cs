using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Delegate;
using Autofac.Core.Activators.ProvidedInstance;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using Xamarin.Forms;

namespace Prism.Autofac.Forms.Extensions
{
    public class DependencyServiceRegistrationSource : IRegistrationSource
    {

        public bool IsAdapterForIndividualComponents => false;

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {

            var swt = service as IServiceWithType;
            if (swt == null)
            {
                return Enumerable.Empty<IComponentRegistration>();
            }

            var instance = ResolveFromDependencyService(swt.ServiceType);

            var registration = new ComponentRegistration(
                Guid.NewGuid(),
                new ProvidedInstanceActivator(instance),
                new CurrentScopeLifetime(),
                InstanceSharing.None,
                InstanceOwnership.OwnedByLifetimeScope,
                new[] { service },
                new Dictionary<string, object>());

            return new IComponentRegistration[] { registration };
        }

        private static object ResolveFromDependencyService(Type targetType)
        {
            if (targetType.GetTypeInfo().IsInterface)
            {
                MethodInfo method = typeof(DependencyService).GetTypeInfo().GetDeclaredMethod("Get");
                MethodInfo genericMethod = method.MakeGenericMethod(targetType);
                return genericMethod.Invoke(null, new object[] { DependencyFetchTarget.GlobalInstance });
            }
            return null;
        }
    }
}
