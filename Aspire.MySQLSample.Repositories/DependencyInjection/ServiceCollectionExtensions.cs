using Aspire.MySQLSample.Core;

using Aspire.MySQLSample.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddMySQLSampleRepository(
		this IServiceCollection services)
		=> services
		.AddSingleton(TimeProvider.System)
		.AddScoped<IUserIdGenerator, UserIdGenerator>()
		.AddScoped<IPasswordHasher, PasswordHasher>()
		.AddTransient<IUserRepository, UserRepository>();
}
