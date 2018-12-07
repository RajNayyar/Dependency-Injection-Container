using System;
using Xunit;
using FluentAssertions;
using DIContainers;

namespace DIContianer.Fixtures
{
    public class ContainerFixture
    {
        [Fact]
        public void Instantiating_a_container_test()
        {
            var container = new DIContainer();
            container.Should().NotBeNull();
        }

        [Fact]
        public void Registering_a_dependency_test()
        {
            var container = new DIContainer();
            container.Register(typeof(IX), typeof(X));
            var registration = container.GetRegistration(typeof(IX));
            registration.ContractType.Should().Be(typeof(IX));
            registration.ImplementationType.Should().Be(typeof(X));
        }

        [Fact]
        public void Getting_missing_registration_should_return_null_test()
        {
            var container = new DIContainer();
            var registration = container.GetRegistration(typeof(IX));
            registration.Should().BeNull();
        }

        [Fact]
        public void Registrating_the_same_type_twice_should_override_the_previous_one()
        {
            var container = new DIContainer();
            container.Register(typeof(IX), typeof(X));
            container.Register(typeof(IX), typeof(XAlt));
            var registration = container.GetRegistration(typeof(IX));            
            registration.ContractType.Should().Be(typeof(IX));
            registration.ImplementationType.Should().Be(typeof(XAlt));
        }
    }

    public interface IX { }

    public class X : IX { }

    public class XAlt : IX { }
}
