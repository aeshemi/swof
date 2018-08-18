using Microsoft.AspNetCore.Mvc;

namespace SWOF.Api.Controllers
{
	[Route("api/ping")]
	public class PingController
	{
		/// <summary>
		/// API test endpoint
		/// </summary>
		/// <response code="200">Returns "pong"</response>
		[HttpGet]
		[ProducesResponseType(200)]
		public string Get()
		{
			return "pong";
		}
	}
}
