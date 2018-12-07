using System;

namespace DIContainers
{
    public class TypeRegistration
    {
        public Type ContractType { get; set; }

        public Type ImplementationType { get; set; }

        public string Name { get; set; }
    }
}