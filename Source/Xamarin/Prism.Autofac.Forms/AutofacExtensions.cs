using System;
using Autofac;
using Prism.Common;
using Xamarin.Forms;

namespace Prism.Autofac.Forms
{
    public static class AutofacExtensions
    {
        /// <summary>
        /// Registers a Page for navigation using a convention based approach, which uses the name of the Type being passed in as the unique name.
        /// </summary>
        /// <typeparam name="T">The Type of Page to register</typeparam>
        public static void RegisterTypeForNavigation<T>(this ContainerBuilder builder) where T : Page
        {
            builder.RegisterTypeForNavigation<T>(typeof(T).Name);
        }

        /// <summary>
        /// Registers a Page for navigation.
        /// </summary>
        /// <typeparam name="T">The Type of Page to register</typeparam>
        /// <param name="name">The unique name to register with the Page</param>
        public static void RegisterTypeForNavigation<T>(this ContainerBuilder builder, string name) where T : Page
        {
            Type type = typeof(T);
            builder.RegisterType(type).Named<object>(name);

            PageNavigationRegistry.Register(name, type);
        }

        /// <summary>
        /// Registers a Page for navigation.
        /// </summary>
        /// <typeparam name="T">The Type of Page to register</typeparam>
        /// <typeparam name="C">The Class to use as the unique name for the Page</typeparam>
        /// <param name="kernel"></param>
        public static void RegisterTypeForNavigation<T, C>(this ContainerBuilder builder)
            where T : Page
            where C : class
        {
            Type type = typeof(T);
            string name = typeof(C).FullName;

            builder.RegisterType(type).Named<object>(name);

            PageNavigationRegistry.Register(name, type);
        }
    }
}