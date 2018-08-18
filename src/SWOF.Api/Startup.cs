using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using SWOF.Api.Repositories;
using SWOF.Api.Services;

namespace SWOF.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc()
				.AddMvcOptions(options => options.OutputFormatters.Remove(options.OutputFormatters.OfType<StringOutputFormatter>().First()))
				.AddJsonOptions(options =>
				{
					options.SerializerSettings.Converters.Add(new StringEnumConverter());
					options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					options.SerializerSettings.Formatting = Formatting.Indented;
					options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
				});

			services.AddSingleton<Func<IDbConnection>>(x => () => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));

			services.AddSingleton<IEngineerRepository, SqlEngineerRepository>();
			services.AddSingleton<IScheduleRepository, SqlScheduleRepository>();

			services.AddSingleton<IScheduleService, ScheduleService>();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "Support Wheel of Fate API", Version = "v1" });
				c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "SWOF.Api.xml"));
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCors(builder =>
				builder
					.AllowAnyOrigin()
					.AllowAnyHeader()
					.WithMethods("GET", "PUT", "POST", "DELETE")
			);

			app.UseMvc();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.RoutePrefix = "api/docs";
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Support Wheel of Fate API");
			});
		}
	}
}
