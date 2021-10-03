using System;
using System.Collections.Generic;
using System.Linq;

namespace OopExpressionParser
{
    internal static class ReflectionEx
    {
        internal static T[] CreateAllSubclassesOf<T>()
        {
            return typeof(T).Assembly.GetTypes()
                .Where(it => it.IsSubclassOf(typeof(T)))
                .Select(it => (T) Activator.CreateInstance(it)!)
                .ToArray();
        }

        internal static T[] CreateAllSubclassesOfInterface<T>(Type @interface)
        {
            return GetAllSubclassesOfInterface(@interface)
                .Select(it => (T) Activator.CreateInstance(it)!)
                .ToArray();
        }

        internal static IEnumerable<Type> GetAllSubclassesOfInterface(Type @interface)
        {
            return @interface.Assembly.GetTypes()
                .Where(it => it.GetInterfaces().Any(inter => Equal(inter, @interface)));
        }

        internal static T[] CreateAllSubclassesOfGeneric<T>(Type generic)
        {
            return generic.Assembly.GetTypes()
                .Where(it => IsSubclassOfRawGeneric(generic, it))
                .Select(it => (T) Activator.CreateInstance(it)!)
                .ToArray();
        }

        internal static bool IsSubclassOfRawGeneric(Type generic, in Type toCheck)
        {
            if (generic == toCheck) return false;

            Type? check = toCheck;
            while (check != null && check != typeof(object))
            {
                if (Equal(check, generic) 
                    || (generic.IsInterface && check.GetInterfaces().Any(inter => Equal(inter, generic))))
                {
                    return true;
                }

                check = check.BaseType;
            }

            return false;
        }

        internal static bool Equal(Type a, Type b, bool compareGeneric = true)
        {
            if (!compareGeneric) return a == b;
            a = a.IsGenericType ? a.GetGenericTypeDefinition() : a;
            b = b.IsGenericType ? b.GetGenericTypeDefinition() : b;
            return a == b;
        }

    }
}