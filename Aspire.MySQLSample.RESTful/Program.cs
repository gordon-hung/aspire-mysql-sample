using Aspire.MySQLSample.Core.ApplicationServices;
using Aspire.MySQLSample.MigrationEntry.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddMySqlDbContext<MySqlContext>("mysqldb");
builder.Services.AddDbContextFactory<MySqlContext>();

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

var factory = app.Services.GetRequiredService<IDbContextFactory<MySqlContext>>();
using var context = await factory.CreateDbContextAsync().ConfigureAwait(false);
context.Database.Migrate();

using (var scope = app.Services.CreateScope())
{
	var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
	var username = "Gordon_Hung";
	var password = "1qaz2wsx";

	if (await mediator.Send(new UserGetByUsernameRequest(username)).ConfigureAwait(false) is null)
	{
		using var cts = new CancellationTokenSource();
		await mediator.Send(new UserAddRequest(username, password), cts.Token).ConfigureAwait(false);
	}
}

app.Run();
