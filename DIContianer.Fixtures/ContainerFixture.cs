using System;
using Xunit;
using FluentAssertions;
using DIContainers;
using System.Linq;

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

        [Fact]
        public void Resolve_via_default_constructor_test()
        {
            var container = new DIContainer();
            container.Register(typeof(IX), typeof(X));
            var x = container.Build(typeof(IX));
            x.Should().NotBeNull();
        }

        [Fact]
        public void Resolve_via_constructor_test()
        {
            var container = new DIContainer();
            container.Register(typeof(IX), typeof(XAlt));
            var x = container.Build(typeof(IX)) as XAlt;
            x.Should().NotBeNull();
            x.Inner.Should().NotBeNull();
        }


        [Fact]
        public void Building_non_registered_concrete_type_should_create_instance_test()
        {
            var container = new DIContainer();
            var x = container.Build(typeof(X)) as X;
            x.Should().NotBeNull();
        }

        [Fact]
        public void Building_non_registered_contract_type_should_throw()
        {
            var container = new DIContainer();
            Action create = () => container.Build(typeof(IX));
            create.Should().Throw<DIContainerException>();
        }

        [Fact]
        public void Basic_property_injection_test()
        {
            var container = new DIContainer();
            container
                .Register<IX, XWithProperty>()
                .Register<IY, Y>();
            var x = container.Build<IX>() as XWithProperty;
            x.Should().NotBeNull();
            x.Y.Should().NotBeNull();
        }

        [Fact]
        public void Property_injection_should_only_inject_attributed_properties()
        {
            var container = new DIContainer();
            container
                .Register<IX, XWithMultipleProperties>()
                .Register<IY, Y>();
            var x = container.Build<IX>() as XWithMultipleProperties;
            x.Should().NotBeNull();
            x.Y1.Should().NotBeNull();
            x.Y2.Should().BeNull();
        }

        [Fact]
        public void Multiple_type_registrations_test()
        {
            var container = new DIContainer();
            container
                .Register<IX, X>("x")
                .Register<IX, XAlt>("xalt");
            var registrations = container.GetRegistrations(typeof(IX));
            registrations.Length.Should().Be(2);
            Array
                .ConvertAll(registrations, x => x.Name)
                .Should()
                .Contain(new[] { "x", "xalt" });

        }

        [Fact]
        public void Multiple_type_registrations_should_support_named_build_test()
        {
            var container = new DIContainer();
            container
                .Register<IX, X>("x")
                .Register<IX, XAlt>("xalt");
            container.Build<IX>("x").Should().BeOfType<X>();
            container.Build<IX>("xalt").Should().BeOfType<XAlt>();

        }

    }
}
