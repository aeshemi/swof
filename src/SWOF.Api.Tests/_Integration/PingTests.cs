using FluentAssertions;
using Xunit;

namespace SWOF.Api.Tests._Integration
{
	[Collection("ApiTest")]
	public class PingTests
	{
		private readonly ApiServer apiServer;

		public PingTests(ApiServer apiServer)
		{
			this.apiServer = apiServer;
		}

		[Fact]
		public void Ping()
		{
			var response = apiServer.Client.GetAsync("/api/ping").Result;

			response.EnsureSuccessStatusCode();

			var result = response.Content.ReadAsJsonAsync<string>().Result;

			result.Should().Be("pong");
		}
	}
}
