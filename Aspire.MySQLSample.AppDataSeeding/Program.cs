using System.Net.Mime;
using System.Text.Json.Serialization;
using Aspire.MySQLSample.Core.ApplicationServices;
using Aspire.MySQLSample.MigrationEntry.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

builder.Services
	.AddMySQLSampleCore()
	.AddMySQLSampleRepository();

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

using (var scope = app.Services.CreateScope())
{
	var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
	var password = "1qaz2wsx";

	if (await mediator.Send(new UserGetByUsernameRequest("gordon.hung")).ConfigureAwait(false) is null)
	{
		using var cts = new CancellationTokenSource();
		await mediator.Send(new UserAddRequest("gordon.hung", password), cts.Token).ConfigureAwait(false);
	}

	if (await mediator.Send(new UserGetByUsernameRequest("andy.lin")).ConfigureAwait(false) is null)
	{
		using var cts = new CancellationTokenSource();
		await mediator.Send(new UserAddRequest("andy.lin", password), cts.Token).ConfigureAwait(false);
	}
}

app.Run();
