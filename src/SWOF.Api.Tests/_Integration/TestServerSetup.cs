using System;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace SWOF.Api.Tests._Integration
{
	public class ApiServer : IDisposable
	{
		private readonly TestServer Instance;

		public HttpClient Client { get; }

		public ApiServer()
		{
			Instance = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<Startup>());
			Client = Instance.CreateClient();
		}

		public void Dispose()
		{
			Client.Dispose();
			Instance.Dispose();
		}
	}

	[CollectionDefinition("ApiTest")]
	public class ApiServerCollection : ICollectionFixture<ApiServer>
	{
	}
}
