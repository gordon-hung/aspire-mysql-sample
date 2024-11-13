using System.Net.Mime;
using System.Text.Json.Serialization;
using Aspire.MySQLSample.MigrationEntry.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services
	.AddRouting(options => options.LowercaseUrls = true)
	.AddControllers(options =>
	{
		options.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
		options.Filters.Add(new ConsumesAttribute(MediaTypeNames.Application.Json));
	})
	.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	foreach (var xmlFileName in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml"))
	{
		c.IncludeXmlComments(xmlFileName);
	}
});

builder.AddMySqlDbContext<MySqlContext>("mysqldb");
builder.Services.AddDbContextFactory<MySqlContext>(options =>
	options.UseMySql(
		serverVersion: new MySqlServerVersion(new Version(9, 0, 0)),
		mySqlOptionsAction: option => option
		.MigrationsHistoryTable("__migrations_history")
		.MigrationsAssembly("Aspire.MassTransitMySQL.Sample.MigrationEntry"))
	.LogTo(Console.WriteLine, LogLevel.Information)
	.EnableSensitiveDataLogging()
	.EnableDetailedErrors());

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var factory = app.Services.GetRequiredService<IDbContextFactory<MySqlContext>>();
using var context = await factory.CreateDbContextAsync().ConfigureAwait(false);
context.Database.Migrate();

app.Run();
