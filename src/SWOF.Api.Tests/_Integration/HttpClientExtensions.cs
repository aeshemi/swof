using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using FluentAssertions;
using SWOF.Api.Models;

namespace SWOF.Api.Tests._Integration
{
	public static class HttpClientExtensions
	{
		public static HttpResponseMessage Get(this HttpClient client, string path)
		{
			return client.GetAsync(path).Result;
		}

		public static HttpResponseMessage PostJson(this HttpClient client, string path, object model = null)
		{
			return client.PostAsJsonAsync(path, model).Result;
		}

		public static HttpResponseMessage PutJson(this HttpClient client, string path, object model = null)
		{
			return client.PutAsJsonAsync(path, model).Result;
		}

		public static HttpResponseMessage Delete(this HttpClient client, string path)
		{
			return client.DeleteAsync(path).Result;
		}

		public static HttpResponseMessage AssertStatus(this HttpResponseMessage response, HttpStatusCode code)
		{
			response.StatusCode.Should().Be(code);
			return response;
		}

		public static HttpResponseMessage ShouldBeHttpCreatedResult(this HttpResponseMessage response)
		{
			response.AssertStatus(HttpStatusCode.Created);
			return response;
		}

		public static HttpResponseMessage ShouldBeHttpBadRequest(this HttpResponseMessage response)
		{
			response.AssertStatus(HttpStatusCode.BadRequest);
			return response;
		}

		public static HttpResponseMessage ShouldHaveValidationErrorMessage(this HttpResponseMessage response, string message)
		{
			var error = response
				.ShouldBeHttpBadRequest()
				.Model<ValidationError>();

			error.Should().NotBeNull("Should return a ValidationError response");

			error.Message.Should().Be(message, $"The validation error message: {message}");

			return response;
		}

		public static T Model<T>(this HttpResponseMessage response)
		{
			return response.Content.ReadAsJsonAsync<T>().Result;
		}

		public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
		{
			return await content.ReadAsAsync<T>(GetJsonFormatters());
		}

		private static IEnumerable<MediaTypeFormatter> GetJsonFormatters()
		{
			yield return new JsonMediaTypeFormatter();
		}
	}
}
