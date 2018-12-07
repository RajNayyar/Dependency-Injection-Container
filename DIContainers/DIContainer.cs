using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DIContainers
{


    public class DIContainer
    {
        private Dictionary<Type, Type> _registrations = new Dictionary<Type, Type>();

        public void Register(Type contract, Type concreteType)
        {
            _registrations[contract] = concreteType;
        }

        public object Build(Type type)
        {
            // This should create an instance of the given type
            // How do we do that?
            // 1. If instance is interface or abstract, or hasa mapping defined 
            // return the mapping type
            // 2. If the instance has a default contstructor, invoke it.
            // 3. Else identify constructor parameters, build them first and then use them
            // to create the required object.
            // 4. Inject any property level dependencies.
            var typeToCreate = ResolveType(type);
            var defaultConstructor = typeToCreate.GetConstructors()
                                            .Where(c => c.GetParameters().Length == 0)
                                            .SingleOrDefault();
            var hasDefaultConstructor = defaultConstructor != null;
            object instance = null;
            if (hasDefaultConstructor == true)
                instance = CreateWithDefaultConstructor(typeToCreate);
            else
                instance = CreateWithParameterisedConstructor(typeToCreate);

            InitializePropertyDependencies(instance);
            return instance;
        }

        private void InitializePropertyDependencies(object instance)
        {
            var type = instance.GetType();
            type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite == true)
                .Where( p => p.IsDefined(typeof(DependencyAttribute)))
                .ToList()
                .ForEach(p => p.SetValue(instance, Build(p.PropertyType)));
        }

        public TypeRegistration GetRegistration(Type type)
        {
            if (_registrations.TryGetValue(type, out Type implType) == false)
                return null;
            else
                return new TypeRegistration { ContractType = type, ImplementationType = implType };
        }

        private object CreateWithParameterisedConstructor(Type typeToCreate)
        {
            var constr = typeToCreate
                                                .GetConstructors()
                                                .OrderBy(c => c.GetParameters().Length)
                                                .FirstOrDefault();
            var parameterValues = constr
                                    .GetParameters()
                                    .Select(p => this.Build(p.ParameterType))
                                    .ToArray();
            return constr.Invoke(parameterValues);
        }

        private static object CreateWithDefaultConstructor(Type typeToCreate)
        {
            return Activator.CreateInstance(typeToCreate);
        }

        private Type ResolveType(Type type)
        {
            var isInterfaceOrAbstractType = type.IsInterface || type.IsAbstract;
            var isMapped = _registrations.ContainsKey(type);
            var shouldReturnMapping = isInterfaceOrAbstractType || isMapped;
            if (shouldReturnMapping == true)
            {
                if (_registrations.TryGetValue(type, out Type implType) == false)
                    throw new DIContainerException($"{type.Name} is not registered.");
                return implType;
            }
            else return type;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DependencyAttribute : Attribute
    {

    }
}
