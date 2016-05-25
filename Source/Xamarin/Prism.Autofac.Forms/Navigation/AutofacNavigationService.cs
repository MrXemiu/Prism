using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Prism.Common;
using Prism.Navigation;
using Xamarin.Forms;

namespace Prism.Autofac.Forms.Navigation
{
    public class AutofacNavigationService : PageNavigationService
    {
        private readonly IComponentContext _context;

        public AutofacNavigationService(IComponentContext context, IApplicationProvider applicationProvider) : base(applicationProvider)
        {
            _context = context;
        }

        protected override Page CreatePage(string name)
        {
            return _context.ResolveOptionalNamed<object>(name) as Page;
        }
    }
}

