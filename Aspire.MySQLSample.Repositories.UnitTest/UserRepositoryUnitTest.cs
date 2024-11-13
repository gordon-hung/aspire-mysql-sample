using Aspire.MySQLSample.Core.Enums;
using Aspire.MySQLSample.MigrationEntry.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NSubstitute;

namespace Aspire.MySQLSample.Repositories.UnitTest;

public class UserRepositoryUnitTest
{
	[Fact]
	public async Task Add_User_Does_Not_Exist()
	{
		var options = new DbContextOptionsBuilder<MySqlContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(Add_User_Does_Not_Exist)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;
		var context = new MySqlContext(options);
		_ = context.Database.EnsureDeleted();

		var fakeTimeProvider = Substitute.For<TimeProvider>();

		var id = Guid.NewGuid().ToString("N");
		var username = "username";
		var hashedPassword = "hashedPassword";

		var currentAt = DateTimeOffset.UtcNow;
		_ = fakeTimeProvider.GetUtcNow().Returns(currentAt);

		var sut = new UserRepository(context, fakeTimeProvider);

		await sut.AddAsync(id, username, hashedPassword);

		var actual = await context.Users.Where(x => x.Id == id).SingleAsync();

		Assert.NotNull(actual);
		Assert.Equal(id, actual.Id);
		Assert.Equal(username, actual.Username);
		Assert.Equal(hashedPassword, actual.Password);
	}

	[Fact]
	public async Task Add_User_Already_Exists()
	{
		var options = new DbContextOptionsBuilder<MySqlContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(Add_User_Already_Exists)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;
		var context = new MySqlContext(options);
		_ = context.Database.EnsureDeleted();

		var fakeTimeProvider = Substitute.For<TimeProvider>();

		var id = Guid.NewGuid().ToString("N");
		var username = "username";
		var hashedPassword = "hashedPassword";
		var currentAt = DateTimeOffset.UtcNow;

		var entity = new User
		{
			Id = id,
			Username = username,
			Password = hashedPassword,
			State = (int)UserState.Enable,
			CreatedAt = currentAt.UtcDateTime,
			UpdateAt = currentAt.UtcDateTime
		};
		var cancellationTokenSource = new CancellationTokenSource();
		var token = cancellationTokenSource.Token;
		await context.Users.AddAsync(entity, token);
		await context.SaveChangesAsync(token);

		_ = fakeTimeProvider.GetUtcNow().Returns(currentAt);

		var sut = new UserRepository(context, fakeTimeProvider);

		await Assert.ThrowsAsync<InvalidOperationException>(() => sut.AddAsync(id, username, hashedPassword).AsTask());
	}

	[Fact]
	public async Task Get_User_Already_Exists()
	{
		var options = new DbContextOptionsBuilder<MySqlContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(Get_User_Already_Exists)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;
		var context = new MySqlContext(options);
		_ = context.Database.EnsureDeleted();

		var fakeTimeProvider = Substitute.For<TimeProvider>();

		var id = Guid.NewGuid().ToString("N");
		var username = "username";
		var hashedPassword = "hashedPassword";
		var currentAt = DateTimeOffset.UtcNow;

		var entity = new User
		{
			Id = id,
			Username = username,
			Password = hashedPassword,
			State = (int)UserState.Enable,
			CreatedAt = currentAt.UtcDateTime,
			UpdateAt = currentAt.UtcDateTime
		};
		var cancellationTokenSource = new CancellationTokenSource();
		var token = cancellationTokenSource.Token;
		await context.Users.AddAsync(entity, token);
		await context.SaveChangesAsync(token);

		_ = fakeTimeProvider.GetUtcNow().Returns(currentAt);

		var sut = new UserRepository(context, fakeTimeProvider);

		var actual = await sut.GetAsync(id);

		Assert.NotNull(actual);
		Assert.Equal(id, actual.Id);
		Assert.Equal(username, actual.Username);
		Assert.Equal(hashedPassword, actual.Password);
	}

	[Fact]
	public async Task Get_User_Does_Not_Exist()
	{
		var options = new DbContextOptionsBuilder<MySqlContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(Get_User_Does_Not_Exist)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;
		var context = new MySqlContext(options);
		_ = context.Database.EnsureDeleted();

		var fakeTimeProvider = Substitute.For<TimeProvider>();

		var id = Guid.NewGuid().ToString("N");

		var sut = new UserRepository(context, fakeTimeProvider);

		var actual = await sut.GetAsync(id);

		Assert.Null(actual);
	}

	[Fact]
	public async Task GetByUsername_User_Already_Exists()
	{
		var options = new DbContextOptionsBuilder<MySqlContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(GetByUsername_User_Already_Exists)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;
		var context = new MySqlContext(options);
		_ = context.Database.EnsureDeleted();

		var fakeTimeProvider = Substitute.For<TimeProvider>();

		var id = Guid.NewGuid().ToString("N");
		var username = "username";
		var hashedPassword = "hashedPassword";
		var currentAt = DateTimeOffset.UtcNow;

		var entity = new User
		{
			Id = id,
			Username = username,
			Password = hashedPassword,
			State = (int)UserState.Enable,
			CreatedAt = currentAt.UtcDateTime,
			UpdateAt = currentAt.UtcDateTime
		};
		var cancellationTokenSource = new CancellationTokenSource();
		var token = cancellationTokenSource.Token;
		await context.Users.AddAsync(entity, token);
		await context.SaveChangesAsync(token);

		_ = fakeTimeProvider.GetUtcNow().Returns(currentAt);

		var sut = new UserRepository(context, fakeTimeProvider);

		var actual = await sut.GetByUsernameAsync(username);

		Assert.NotNull(actual);
		Assert.Equal(id, actual.Id);
		Assert.Equal(username, actual.Username);
		Assert.Equal(hashedPassword, actual.Password);
	}

	[Fact]
	public async Task GetByUsername_User_Does_Not_Exist()
	{
		var options = new DbContextOptionsBuilder<MySqlContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(GetByUsername_User_Does_Not_Exist)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;
		var context = new MySqlContext(options);
		_ = context.Database.EnsureDeleted();

		var fakeTimeProvider = Substitute.For<TimeProvider>();

		var username = "username";

		var sut = new UserRepository(context, fakeTimeProvider);

		var actual = await sut.GetByUsernameAsync(username);

		Assert.Null(actual);
	}
}
