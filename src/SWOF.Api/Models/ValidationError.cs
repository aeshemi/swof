namespace SWOF.Api.Models
{
	public class ValidationError
	{
		public ValidationError()
		{
		}

		public ValidationError(string message, string key = null)
		{
			Message = message;
			Key = key;
		}

		public string Key { get; set; }
		public string Message { get; set; }
	}
}
