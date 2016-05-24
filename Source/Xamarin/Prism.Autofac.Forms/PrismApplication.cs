using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using Autofac.Features.ResolveAnything;
using Prism.Autofac.Forms.Extensions;
using Prism.Autofac.Forms.Modularity;
using Prism.Autofac.Forms.Navigation;
using Prism.Common;
using Prism.Events;
using Prism.Logging;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;
using DependencyService = Prism.Services.DependencyService;

namespace Prism.Autofac.Forms
{
    public abstract class PrismApplication : PrismApplicationBase
    {
        private IContainer _container;

        public IContainer Container
        {
            get
            {
                if(_container == null)
                    ConfigureContainer();
                return _container;
            }
            private set { _container = value; }
        }

        public override void Initialize()
        {
            ConfigureContainer();

            ModuleCatalog = CreateModuleCatalog();

            ConfigureModuleCatalog();

            NavigationService = CreateNavigationService();

            RegisterTypes();

            InitializeModules();
        }

        protected override void InitializeModules()
        {
            if (!ModuleCatalog.Modules.Any()) return;

            IModuleManager manager = Container.Resolve<IModuleManager>();
            manager.Run();
        }

        protected override INavigationService CreateNavigationService()
        {
            return Container.Resolve<INavigationService>();
        }

        protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) =>
            {
                List<Parameter> overrides = null;

                var page = view as Page;
                if (page != null)
                {
                    var navService = Container.Resolve<AutofacNavigationService>();
                    ((IPageAware)navService).Page = page;

                    overrides = new List<Parameter>
                    {
                        new NamedParameter("navigationService", navService)
                    };
                }

                return Container.Resolve(type, overrides);
            });
        }

        private void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.Register(context => Logger).As<ILoggerFacade>().SingleInstance();
            builder.Register(context => ModuleCatalog).As<IModuleCatalog>().SingleInstance();

            builder.RegisterType<ApplicationProvider>().As<IApplicationProvider>().SingleInstance();
            builder.RegisterType<AutofacNavigationService>().Named<INavigationService>("navigationService");
            builder.RegisterType<ModuleManager>().As<IModuleManager>().SingleInstance();
            builder.RegisterType<AutofacModuleInitializer>().As<IModuleInitializer>().SingleInstance();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<DependencyService>().As<IDependencyService>().SingleInstance();
            builder.RegisterType<PageDialogService>().As<IPageDialogService>().SingleInstance();

            ConfigureContainer(ref builder);

            builder.RegisterSource(new DependencyServiceRegistrationSource());

            Container =  builder.Build();
        }

        public virtual void ConfigureContainer(ref ContainerBuilder builder) { }
    }
}
