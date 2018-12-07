using System;

namespace DIContainers
{
    public class TypeKey : IEquatable<TypeKey>
    {
        public TypeKey(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public Type Type { get; }

        public string Name { get; }

        public override int GetHashCode()
        {
            return Name == null ? Type.GetHashCode() : Type.GetHashCode() ^ Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is TypeKey other) ? Equals(other) : false;
        }

        public bool Equals(TypeKey other)
        {
            if (other == null) return false;
            return Type.Equals(other.Type)
                && (Name ?? string.Empty).Equals(other.Name ?? string.Empty);
        }
    }

}
