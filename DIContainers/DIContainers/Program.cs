﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DIContainers
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new DIContainer();
            container.Register(typeof(Bread), typeof(SesameSeedBun));
            container.Register(typeof(Cheese), typeof(SwissCheese));
            container.Register(typeof(Sauce), typeof(Ketchup));
            container.Register(typeof(Patty), typeof(ChickenPatty));
            var burger = container.Build(typeof(Burger));
            Console.ReadKey(true);
        }
            

        public class Burger
        {
            public Burger(Bread bread, Patty patty, Sauce sauce, Cheese cheese)
            {
                Bread = bread;
                Patty = patty;
                Sauce = sauce;
                Cheese = cheese;
            }

            public Bread Bread { get; set; }

            public Patty Patty { get; set; }

            public Sauce Sauce { get; set; }

            public Cheese Cheese { get; set; }
        }

        public class WheatBread : Bread { }

        public class SesameSeedBun : Bread { }

        public class ChickenPatty : Patty { }

        public class MixedVegPatty : Patty { }

        public class Mustard : Sauce { }

        public class Ketchup : Sauce { }

        public class CheeddarCheese : Cheese { }

        public class SwissCheese : Cheese { }

        public abstract class Bread { }
        public abstract class Patty { }
        public abstract class Sauce { }
        public abstract class Cheese { }

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
                var typeToCreate = ResolveType(type);
                var defaultConstructor = typeToCreate.GetConstructors()
                                                .Where(c => c.GetParameters().Length == 0)
                                                .SingleOrDefault();
                var hasDefaultConstructor = defaultConstructor != null;
                if (hasDefaultConstructor == true)
                    return CreateWithDefaultConstructor(typeToCreate);
                else
                    return CreateWithParameterisedConstructor(typeToCreate);
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
                if( shouldReturnMapping == true )
                    return _registrations[type];
                else return type;
            }
        }
    }
}
