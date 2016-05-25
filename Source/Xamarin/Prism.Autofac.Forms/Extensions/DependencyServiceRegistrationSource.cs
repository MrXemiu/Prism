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
using Prism.Services;
using Xamarin.Forms;
using DependencyService = Xamarin.Forms.DependencyService;

namespace Prism.Autofac.Forms.Extensions
{
    /// <summary>
    /// Searches Xamarin.Forms.DependencyService for Dependencies that aren't registered with Autofac
    /// </summary>
    public class DependencyServiceRegistrationSource : IRegistrationSource
    {

        public bool IsAdapterForIndividualComponents => false;

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            var registrations = registrationAccessor(service).ToList();

            if (registrations.Any()) return registrations;

            var swt = service as IServiceWithType;
            if (swt == null)
            {
                return Enumerable.Empty<IComponentRegistration>();
            }

            var method = typeof(DependencyService).GetTypeInfo().GetDeclaredMethod("Get");
            var genericMethod = method.MakeGenericMethod(swt.ServiceType);
            var dependencyService = genericMethod.Invoke(null, new object []{ DependencyFetchTarget.GlobalInstance });

            if (dependencyService != null)
            {

                var registration = new ComponentRegistration(
                    Guid.NewGuid(),
                    new ProvidedInstanceActivator(dependencyService),
                    new CurrentScopeLifetime(),
                    InstanceSharing.None,
                    InstanceOwnership.OwnedByLifetimeScope,
                    new[] {service},
                    new Dictionary<string, object>());

                return new IComponentRegistration[] {registration};
            }

            return Enumerable.Empty<IComponentRegistration>();
        }
    }
}
