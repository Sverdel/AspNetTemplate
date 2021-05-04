using System.Net;
using System.Threading.Tasks;
using AspNetTemplate.IntegrationTests.Setup;
using FluentAssertions;
using Xunit;

namespace AspNetTemplate.IntegrationTests
{
    public class PingTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public PingTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task PingTest()
        {
            var response = await _fixture.CreateClient().GetAsync("ping");
            response.StatusCode.Should().Be(HttpStatusCode.OK, await response.Content.ReadAsStringAsync());
            var responseDto = await response.Content.ReadAsStringAsync();
            responseDto.Should().Be("pong");
        }
    }
}