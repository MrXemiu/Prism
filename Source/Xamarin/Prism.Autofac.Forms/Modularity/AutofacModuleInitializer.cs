using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Prism.Modularity;

namespace Prism.Autofac.Forms.Modularity
{
    public class AutofacModuleInitializer : IModuleInitializer
    {
        private readonly IComponentContext _componentContext;
        public AutofacModuleInitializer(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public void Initialize(ModuleInfo moduleInfo)
        {
            var module = CreateModule(moduleInfo.ModuleType);
            module?.Initialize();
        }

        protected virtual IModule CreateModule(Type moduleType)
        {
            return (IModule) _componentContext.Resolve(moduleType);
        }
    }
}
