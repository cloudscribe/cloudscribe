using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace cloudscribe.WebHost.DI
{
    internal class CommonConventions
    {
/// <summary>
/// Registers all single types in a set of assemblies that meet the following criteria:
/// 1) The name of the type matches the interface name (I[TypeName] = [TypeName]) or 
/// 2) The name of the type matches the interface name + the suffix "Adapter" (I[TypeName] = [TypeName]Adapter)
/// 3) The class does not have a string parameter in the constructor
/// 4) The class is not decorated with the [ExcludeFromAutoRegistrationAttribute]
/// </summary>
/// <param name="registerMethod">The method on the DI container used to register a type to an abstraction. 
/// Should be in the format (interfaceType, implementationType) => SomeMethod(interfaceType, implementationType) 
/// or similar.</param>
/// <param name="interfaceAssemblies">An array of assemblies to scan for the interface types. These assemblies 
/// must be referenced within this project. Typically, you will just want to use the MvcSiteMapProvider 
/// assembly unless you decide to use <see cref="CommonConventions"/> throughout your project.</param>
/// <param name="implementationAssemblies">An array of assemblies to scan for the implementation types. 
/// This array should contain all of the assemblies where you have defined types that will be injected 
/// into MvcSiteMapProvider.</param>
/// <param name="excludeTypes">An array of <see cref="System.Type"/> used to manually exclude types if 
/// you intend to register them manually. Use this parameter if you want to inject your own implementation
/// and therefore don't want the default type automatically registered.</param>
/// <param name="excludeRegEx">A regular expression that can be used to exclude types based on the type 
/// name (excluding the namespace name). All types that match the regular expression will be excluded.</param>
        public static void RegisterDefaultConventions(
            Action<Type, Type> registerMethod,
            Assembly[] interfaceAssemblies,
            Assembly[] implementationAssemblies,
            Type[] excludeTypes,
            string excludeRegEx)
        {
            List<Type> interfaces = new List<Type>();

            foreach (var assembly in interfaceAssemblies)
                interfaces.AddRange(GetInterfaces(assembly));

            foreach (var interfaceType in interfaces)
            {
                if (!IsExcludedType(interfaceType, excludeTypes, excludeRegEx))
                {
                    List<Type> implementations = new List<Type>();

                    foreach (var assembly in implementationAssemblies)
                        implementations.AddRange(GetImplementationsOfInterface(assembly, interfaceType).Where(implementation => !IsExcludedType(implementation, excludeTypes, excludeRegEx)).ToArray());

// Prefer the default name ITypeName = TypeName
                    Type implementationType = implementations.Where(implementation => IsDefaultType(interfaceType, implementation)).FirstOrDefault();

                    if (implementationType == null)
                    {
// Fall back on ITypeName = ITypeNameAdapter
                        implementationType = implementations.Where(implementation => IsAdapterType(interfaceType, implementation)).FirstOrDefault();
                    }

                    if (implementationType != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Auto registration of {1} : {0}", interfaceType.Name, implementationType.Name);
                        registerMethod(interfaceType, implementationType);
                    }
                }
            }
        }

// For DI containers that allow the use of a multiple registration method calls for individual implementations of a given interface

/// <summary>
/// Registers all of the types that implement the passed in interfaceTypes with the DI container so they can be 
/// resolved as an <see cref="IEnumerable{T}"/> of values (where T is the interface type).
/// 
/// This overload is for DI containers that allow the use of multiple registration method calls, one for 
/// each implementation of the interface.
/// </summary>
/// <param name="registerMethod">The method of the DI container used to register a single implementation, which will be 
/// called one time per implementation found to register each implementation of the type. Should be in the format 
/// (interfaceType, implementationType) => SomeMethod(interfaceType, implementationType) or similar.</param>
/// <param name="interfaceTypes">The interfaces to limit the registration to. If empty, no types will be registered.</param>
/// <param name="implementationAssemblies">An array of assemblies to scan for the implementation types. 
/// This array should contain all of the assemblies where you have defined types that will be injected 
/// into MvcSiteMapProvider.</param>
/// <param name="excludeTypes">An array of <see cref="System.Type"/> used to manually exclude types if 
/// you intend to register them manually. Use this parameter if you want to inject your own implementation
/// and therefore don't want the default type automatically registered.</param>
/// <param name="excludeRegEx">A regular expression that can be used to exclude types based on the type 
/// name (excluding the namespace name). All types that match the regular expression will be excluded.</param>
        public static void RegisterAllImplementationsOfInterface(
            Action<Type, Type> registerMethod,
            Type[] interfaceTypes,
            Assembly[] implementationAssemblies,
            Type[] excludeTypes,
            string excludeRegEx)
        {
            foreach (var interfaceType in interfaceTypes)
            {
                List<Type> implementations = new List<Type>();

                foreach (var assembly in implementationAssemblies)
                    implementations.AddRange(GetImplementationsOfInterface(assembly, interfaceType));

                foreach (var implementationType in implementations)
                {
                    if (!IsExcludedType(implementationType, excludeTypes, excludeRegEx))
                    {
                        System.Diagnostics.Debug.WriteLine("Auto registration of {1} : {0}", interfaceType.Name, implementationType.Name);
                        registerMethod(interfaceType, implementationType);
                    }
                }
            }
        }

/// <summary>
/// Registers all of the types that implement the passed in interfaceTypes with the DI container so they can be 
/// resolved as an <see cref="IEnumerable{T}"/> of values (where T is the interface type).
/// 
/// This overload is for DI containers that require the use of a multiple registration method call for 
/// all implementations a given interface.
/// </summary>
/// <param name="registerMethod">The method of the DI container used to register an array of implementations, which will be 
/// called only one time to register all of the implementations of the type. Should be in the format 
/// (interfaceType, implementationTypes) => SomeMethod(interfaceType, implementationTypes) or similar, where
/// implementationTypes is a <see cref="System.Collections.Generic.IEnumerable{System.Type}"/>.</param>
/// <param name="interfaceTypes">The interfaces to limit the registration to. If empty, no types will be registered.</param>
/// <param name="implementationAssemblies">An array of assemblies to scan for the implementation types. 
/// This array should contain all of the assemblies where you have defined types that will be injected 
/// into MvcSiteMapProvider.</param>
/// <param name="excludeTypes">An array of <see cref="System.Type"/> used to manually exclude types if 
/// you intend to register them manually. Use this parameter if you want to inject your own implementation
/// and therefore don't want the default type automatically registered.</param>
/// <param name="excludeRegEx">A regular expression that can be used to exclude types based on the type 
/// name (excluding the namespace name). All types that match the regular expression will be excluded.</param>
        public static void RegisterAllImplementationsOfInterfaceSingle(
            Action<Type, IEnumerable<Type>> registerMethod,
            Type[] interfaceTypes,
            Assembly[] implementationAssemblies,
            Type[] excludeTypes,
            string excludeRegEx)
        {
            foreach (var interfaceType in interfaceTypes)
            {
                List<Type> implementations = new List<Type>();
                List<Type> matchingImplementations = new List<Type>();

                foreach (var assembly in implementationAssemblies)
                    implementations.AddRange(GetImplementationsOfInterface(assembly, interfaceType));

                foreach (var implementationType in implementations)
                {
                    if (!IsExcludedType(implementationType, excludeTypes, excludeRegEx))
                    {
                        matchingImplementations.Add(implementationType);
                    }
                }

                System.Diagnostics.Debug.WriteLine("Auto multiple registration of {1} : {0}", interfaceType.Name, string.Join(", ", matchingImplementations.Select(t => t.Name)));
                registerMethod(interfaceType, matchingImplementations);
            }
        }


        private static bool IsExcludedType(Type type, Type[] excludeTypes, string excludeRegEx)
        {
            return IsExcludedType(type, excludeTypes) || IsExcludedType(type, excludeRegEx) || IsExcludedType(type);
        }

        private static bool IsExcludedType(Type type, Type[] excludeTypes)
        {
            return excludeTypes.Contains(type);
        }

        private static bool IsExcludedType(Type type, string excludeRegEx)
        {
            if (string.IsNullOrEmpty(excludeRegEx)) return false;
            return Regex.Match(type.Name, excludeRegEx, RegexOptions.Compiled).Success;
        }

        private static bool IsExcludedType(Type type)
        {
            return type.GetCustomAttributes(typeof(MvcSiteMapProvider.DI.ExcludeFromAutoRegistrationAttribute), false).Length > 0;
        }

        private static bool IsDefaultType(Type interfaceType, Type implementationType)
        {
            return interfaceType.Name.Equals("I" + implementationType.Name);
        }

        private static bool IsAdapterType(Type interfaceType, Type implementationType)
        {
            return implementationType.Name.EndsWith("Adapter") &&
                interfaceType.Name.Equals("I" + implementationType.Name.Substring(0, implementationType.Name.Length - 7));
        }

        private static IEnumerable<Type> GetInterfaces(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsInterface);
        }

        private static IEnumerable<Type> GetImplementationsOfInterface(Assembly assembly, Type interfaceType)
        {
            return assembly.GetTypes().Where(t =>
                !t.IsInterface &&
                !t.IsAbstract &&
                interfaceType.IsAssignableFrom(t) &&
                t.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                    .Any(type => type.GetParameters().Select(p => p.ParameterType).All(p => (p.IsInterface || p.IsClass) && p != typeof(string))));
        }
    }
}
