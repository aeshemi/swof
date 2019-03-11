using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

/****************************************************************************************

SWOF is an API used to generate schedule rotations for engineers.

****************************************************************************************/

/*** CHANGE LOG *************************************************************************

CHANGE LOG:

Formatted to AS standards.

****************************************************************************************/

namespace SWOF.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();
	}
}
