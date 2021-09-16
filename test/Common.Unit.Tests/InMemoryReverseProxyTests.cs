using FluentAssertions;
using PactTestingTools;
using Xunit;

namespace Common.Unit.Tests
{
    public class InMemoryReverseProxyTests
    {
        [Fact]
        public void ReserveAnOpenLocalhostPort_WorksTwice()
        {
            // Act
            int portA = InMemoryReverseProxy.ReserveAnOpenLocalhostPort();
            int portB = InMemoryReverseProxy.ReserveAnOpenLocalhostPort();

            // Assert
            InMemoryReverseProxy._reservedPorts.ContainsKey(portA).Should().BeTrue("because the first returned port should be in the reserved list");
            InMemoryReverseProxy._reservedPorts.ContainsKey(portB).Should().BeTrue("because the second returned port should be in the reserved list");
            portA.Should().NotBe(portB, "because the same port shouldn't be reserved twice");

            // Teardown
            InMemoryReverseProxy.UnreserveAPort(portB);
            InMemoryReverseProxy.UnreserveAPort(portA);
        }

        [Fact]
        public void UnreserveAPort_WorksTwice()
        {
            // Arrange
            int portA = InMemoryReverseProxy.ReserveAnOpenLocalhostPort();
            int portB = InMemoryReverseProxy.ReserveAnOpenLocalhostPort();

            // Act & Assert
            InMemoryReverseProxy.UnreserveAPort(portB);
            InMemoryReverseProxy._reservedPorts.ContainsKey(portB).Should().BeFalse("because the second returned port should no longer be in the reserved list after it's removed");

            InMemoryReverseProxy.UnreserveAPort(portA);
            InMemoryReverseProxy._reservedPorts.ContainsKey(portA).Should().BeFalse("because the second returned port should no longer be in the reserved list after it's removed");
        }
    }
}
