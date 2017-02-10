using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WS.Utilities.Injection
{
    /// <summary>
    /// A very basic dependency injection container that relies on constructor injection
    /// </summary>
    /// <remarks>
    /// Created as already available heavier weight containers were not supported in .NET Core command line applications.
    /// </remarks>
    public class BasicInjectionContainer
    {
        private readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();
        private readonly Dictionary<Type, Type> _typeMap = new Dictionary<Type, Type>();

        /// <summary>
        /// Register a type with the container, which when requested will return an instance of an inherting or implementing type
        /// </summary>
        /// <param name="requestedType">The type that will be requested by a consumer</param>
        /// <param name="providedType">The type of the instance that will be returned by the container</param>
        /// <exception cref="ArgumentException">If the requested type is not assignable from the provided type</exception>
        public void RegisterType(Type requestedType, Type providedType)
        {
            if (!requestedType.IsAssignableFrom(providedType))
            {
                throw new ArgumentException($"{providedType.FullName} cannot be assigned to {requestedType.FullName}", nameof(providedType));
            }
            // Ensure that the provided type has a valid constructor
            GetConstructor(providedType);
            _typeMap[requestedType] = providedType;
        }

        /// <summary>
        /// Register a type with the container, which when requested will return an instance of an inherting or implementing type
        /// </summary>
        /// <typeparam name="TRequestedType">The type that will be requested by a consumer</typeparam>
        /// <typeparam name="TProvidedType">The type of the instance that will be returned by the container</typeparam>
        public void RegisterType<TRequestedType, TProvidedType>() where TProvidedType : TRequestedType
        {
            RegisterType(typeof(TRequestedType), typeof(TProvidedType));
        }

        /// <summary>
        /// Register a type with the container
        /// </summary>
        /// <typeparam name="TRequestedType">The type that will be requested by a consumer and returned by the container</typeparam>
        public void RegisterType<TRequestedType>()
        {
            RegisterType(typeof(TRequestedType), typeof(TRequestedType));
        }

        /// <summary>
        /// Registers an instance of a type with the container
        /// </summary>
        /// <typeparam name="T">The type that will be requested by a consumer</typeparam>
        /// <param name="instance">The instance that will be returned by the container</param>
        public void RegisterInstance<T>(T instance)
        {
            _instances[typeof(T)] = instance;
        }

        /// <summary>
        /// Gets an instance of the specified type, if the type is requested repeatedly the same instance will be returned each time
        /// </summary>
        /// <param name="type">The type requested by the consumer</param>
        /// <returns>An instance of the requested type</returns>
        public object Resolve(Type type)
        {
            lock (_instances)
            {
                if (!_instances.ContainsKey(type))
                {
                    _instances[type] = CreateInstance(type);
                }
                return _instances[type];
            }
        }

        /// <summary>
        /// Gets an instance of the specified type, if the type is requested repeatedly the same instance will be returned each time
        /// </summary>
        /// <typeparam name="T">The type requested by the consumer</typeparam>
        /// <returns>An instance of the requested type</returns>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        private object CreateInstance(Type requestedType)
        {
            if (!_typeMap.ContainsKey(requestedType))
            {
                throw new ArgumentException($"Cannot resolve unknown type: {requestedType.FullName}", nameof(requestedType));
            }
            var providedType = _typeMap[requestedType];
            var arguments = GetConstructor(providedType)
                .GetParameters()
                .Select(p => Resolve(p.ParameterType))
                .ToArray();
            return Activator.CreateInstance(providedType, arguments);
        }

        private static ConstructorInfo GetConstructor(Type providedType)
        {
            var constructors = providedType.GetConstructors();
            if (constructors.Length != 1)
            {
                throw new ArgumentException("Provided type must have a single public or default constructor", nameof(providedType));
            }
            return constructors[0];
        }
    }
}
