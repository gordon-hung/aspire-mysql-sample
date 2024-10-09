using Aspire.MySQLSample.MigrationEntry.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Aspire.MySQLSample.MigrationEntry;

public class MigrationsContextFactory : IDesignTimeDbContextFactory<MySqlContext>
{
	public MySqlContext CreateDbContext(string[] args)
	{
		var host = Host.CreateDefaultBuilder(args)
			.ConfigureHostConfiguration(builder => builder
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables())
			.ConfigureServices(services => services
				.AddDbContext<MySqlContext>(
				dbOptions => dbOptions
				.UseMySql(
					connectionString: "Server=localhost;Database=sample;Uid=root;Pwd=1qaz2wsx!@;",
					serverVersion: new MySqlServerVersion(new Version(9, 0, 0)),
					mySqlOptionsAction: option => option
					//.MigrationsHistoryTable("__migrations_history")
					.MigrationsAssembly("Aspire.MySQLSample.MigrationEntry"))))
			.Build();

		var dbContext = host.Services.GetRequiredService<MySqlContext>();

		return dbContext;
	}
}
