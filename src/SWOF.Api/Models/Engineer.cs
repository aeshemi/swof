using Dapper;

namespace SWOF.Api.Models
{
	[Table("Engineers")]
	public class Engineer
	{
		public int Id { get; set; }

		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public bool IsActive { get; set; }
	}
}
