using System;
using Autofac;
using Autofac.Core;
using Prism.Common;
using Xamarin.Forms;

namespace Prism.Autofac.Forms
{
    public static class AutofacExtensions
    {

        /// <summary>
        /// Registers an instance of T to be stored in the container.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <param name="instance">Instance of type T.</param>
        public static IContainer Register<T>(this IContainer container, T instance) where T : class
        {
            var builder = new ContainerBuilder();
            builder.Register<T>(t => instance).As<T>();
            builder.Update(container);
            return container;
        }

        /// <summary>
        /// Registers a type to instantiate for type T.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <typeparam name="TImpl">Type to register for instantiation.</typeparam>
        public static IContainer Register<T, TImpl>(this IContainer container)
            where T : class
            where TImpl : class, T
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TImpl>().As<T>();
            builder.Update(container);
            return container;
        }

        /// <summary>
        /// Tries to register a type
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <param name="type">Type of implementation</param>
        public static IContainer Register<T>(this IContainer container, Type type) where T : class
        {
            var builder = new ContainerBuilder();
            builder.RegisterType(type).As<T>();
            builder.Update(container);
            return container;
        }

        /// <summary>
        /// Tries to register a type
        /// </summary>
        /// <param name="type">Type to register.</param>
        /// <param name="impl">Type that implements registered type.</param>
        public static IContainer Register(this IContainer container, Type type, Type impl)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType(impl).As(type);
            builder.Update(container);
            return container;
        }


        /// <summary>
        /// Registers a Page for navigation using a convention based approach, which uses the name of the Type being passed in as the unique name.
        /// </summary>
        /// <typeparam name="T">The Type of Page to register</typeparam>
        public static void RegisterTypeForNavigation<T>(this IContainer container) where T : Page
        {
            var builder = new ContainerBuilder();
            builder.RegisterTypeForNavigation<T>(typeof(T).Name);
            builder.Update(container);
        }

        /// <summary>
        /// Registers a Page for navigation.
        /// </summary>
        /// <typeparam name="T">The Type of Page to register</typeparam>
        /// <param name="name">The unique name to register with the Page</param>
        public static void RegisterTypeForNavigation<T>(this IContainer container, string name) where T : Page
        {
            Type type = typeof(T);

            var builder = new ContainerBuilder();
            builder.RegisterType(type).Named<object>(name);
            builder.Update(container);

            PageNavigationRegistry.Register(name, type);
        }

        /// <summary>
        /// Registers a Page for navigation.
        /// </summary>
        /// <typeparam name="T">The Type of Page to register</typeparam>
        /// <typeparam name="C">The Class to use as the unique name for the Page</typeparam>
        /// <param name="kernel"></param>
        public static void RegisterTypeForNavigation<T, C>(this IContainer container)
            where T : Page
            where C : class
        {
            Type type = typeof(T);
            string name = typeof(C).FullName;

            var builder = new ContainerBuilder();
            builder.RegisterType<C>().Named<T>(name);
            builder.Update(container);

            PageNavigationRegistry.Register(name, type);
        }


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