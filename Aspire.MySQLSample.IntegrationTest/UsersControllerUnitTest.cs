using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using Aspire.MySQLSample.AppRESTful.ViewModels;
using Aspire.MySQLSample.Core.ApplicationServices;

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Aspire.MySQLSample.IntegrationTest;

public class UsersControllerUnitTest
{
	private static readonly JsonSerializerOptions _SerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters =
		{
			new JsonStringEnumConverter()
		}
	};

	[Fact]
	public async Task Add_User()
	{
		var source = new UserAddViewModel
		{
			UserName = "username",
			Password = "password"
		};

		var id = "412345679";
		var web = new AppRESTfulApplication(builder =>
		{
			var fakeIMediator = Substitute.For<IMediator>();

			_ = fakeIMediator.Send(
				request: Arg.Is<UserAddRequest>(predicate: compare =>
					compare.Username == source.UserName &&
					compare.Password == source.Password),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(id);

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeIMediator));
		});

		var httpClient = web.CreateDefaultClient();

		var jsonContent = JsonContent.Create(source);
		var httpResponseMessage = await httpClient.PostAsync(
			"/api/users",
			jsonContent);

		var actual = JsonSerializer.Deserialize<string>(json: await httpResponseMessage.Content.ReadAsStringAsync(), options: _SerializerOptions);

		Assert.NotNull(actual);
		Assert.Equal(id, actual);
	}

	[Fact]
	public async Task Get_User_Already_Exists()
	{
		var id = "412345679";
		var username = "username";
		var createdAt = DateTimeOffset.UtcNow;
		var updateAt = DateTimeOffset.UtcNow;
		var web = new AppRESTfulApplication(builder =>
		{
			var fakeIMediator = Substitute.For<IMediator>();

			_ = fakeIMediator.Send(
				request: Arg.Is<UserGetRequest>(predicate: compare =>
					compare.Id == id),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Task.FromResult<UserInfoResponse?>(new UserInfoResponse(
				id,
				username,
				Core.Enums.UserState.Enable,
				createdAt,
				updateAt)));

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeIMediator));
		});

		var httpClient = web.CreateDefaultClient();

		var httpResponseMessage = await httpClient.GetAsync($"/api/users/{id}");

		var actual = JsonSerializer.Deserialize<UserInfoViewModel?>(json: await httpResponseMessage.Content.ReadAsStringAsync(), options: _SerializerOptions);

		Assert.NotNull(actual);
		Assert.Equal(id, actual.Id);
		Assert.Equal(username, actual.Username);
		Assert.Equal(Core.Enums.UserState.Enable, actual.State);
	}

	[Fact]
	public async Task Get_User_Does_Not_Exist()
	{
		var id = "412345679";

		var web = new AppRESTfulApplication(builder =>
		{
			var fakeIMediator = Substitute.For<IMediator>();

			_ = fakeIMediator.Send(
				request: Arg.Is<UserGetRequest>(predicate: compare =>
					compare.Id == id),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Task.FromResult<UserInfoResponse?>(null));

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeIMediator));
		});

		var httpClient = web.CreateDefaultClient();

		var httpResponseMessage = await httpClient.GetAsync($"/api/users/{id}");

		Assert.Equal(System.Net.HttpStatusCode.NoContent, httpResponseMessage.StatusCode);
	}

	[Fact]
	public async Task GetByUsername_User_Already_Exists()
	{
		var id = "412345679";
		var username = "username";
		var createdAt = DateTimeOffset.UtcNow;
		var updateAt = DateTimeOffset.UtcNow;
		var web = new AppRESTfulApplication(builder =>
		{
			var fakeIMediator = Substitute.For<IMediator>();

			_ = fakeIMediator.Send(
				request: Arg.Is<UserGetByUsernameRequest>(predicate: compare =>
					compare.Username == username),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Task.FromResult<UserInfoResponse?>(new UserInfoResponse(
				id,
				username,
				Core.Enums.UserState.Enable,
				createdAt,
				updateAt)));

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeIMediator));
		});

		var httpClient = web.CreateDefaultClient();

		var httpResponseMessage = await httpClient.GetAsync($"/api/users/{username}/username");

		var actual = JsonSerializer.Deserialize<UserInfoViewModel?>(json: await httpResponseMessage.Content.ReadAsStringAsync(), options: _SerializerOptions);

		Assert.NotNull(actual);
		Assert.Equal(id, actual.Id);
		Assert.Equal(username, actual.Username);
		Assert.Equal(Core.Enums.UserState.Enable, actual.State);
	}

	[Fact]
	public async Task GetByUsername_User_Does_Not_Exist()
	{
		var username = "username";

		var web = new AppRESTfulApplication(builder =>
		{
			var fakeIMediator = Substitute.For<IMediator>();

			_ = fakeIMediator.Send(
				request: Arg.Is<UserGetByUsernameRequest>(predicate: compare =>
					compare.Username == username),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Task.FromResult<UserInfoResponse?>(null));

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeIMediator));
		});

		var httpClient = web.CreateDefaultClient();

		var httpResponseMessage = await httpClient.GetAsync($"/api/users/{username}/username");

		Assert.Equal(System.Net.HttpStatusCode.NoContent, httpResponseMessage.StatusCode);
	}
}
