using Microsoft.Extensions.DependencyInjection;
using Sample.OpenTelemetry.WebApi.Core.Middlewares;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Sample.OpenTelemetry.WebApi.Core.Extensions
{
	public static class ApiConfigurationExtensions
	{
		public static void AddApiConfiguration(this IServiceCollection services)
		{
			services.AddRouting(options => options.LowercaseUrls = true);

			services.AddControllers();
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.AddControllers();
		}

		public static void UseApiConfiguration(this WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthorization();
			app.MapControllers();

			app.UseMiddleware<ErrorHandlingMiddleware>();
		}
	}
}