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
    }
}
