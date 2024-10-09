using Aspire.MySQLSample.Core;
using Aspire.MySQLSample.Core.Enums;
using Aspire.MySQLSample.Core.Models;
using Aspire.MySQLSample.MigrationEntry.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aspire.MySQLSample.Repositories;

internal class UserRepository(
	MySqlContext context,
	TimeProvider timeProvider) : IUserRepository
{
	public async ValueTask AddAsync(string id, string username, string hashedPassword, CancellationToken cancellationToken = default)
	{
		var currentAt = timeProvider.GetUtcNow();
		var entity = new User
		{
			Id = id,
			Username = username,
			Password = hashedPassword,
			State = (int)UserState.Enable,
			CreatedAt = currentAt.UtcDateTime,
			UpdateAt = currentAt.UtcDateTime
		};
		await context.Users.AddAsync(entity, cancellationToken).ConfigureAwait(false);

		await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask<UserInfo?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
	=> await context.Users
		.Where(u => u.Id == id)
		.Select(u => new UserInfo(
			u.Id,
			u.Username,
			u.Password,
			(UserState)u.State,
			u.CreatedAt,
			u.UpdateAt))
		.FirstOrDefaultAsync(cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<UserInfo?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
	=> await context.Users
		.Where(u => u.Username == username)
		.Select(u => new UserInfo(
			u.Id,
			u.Username,
			u.Password,
			(UserState)u.State,
			u.CreatedAt,
			u.UpdateAt))
		.FirstOrDefaultAsync(cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask UpdatePasswordAsync(string id, string hashedPassword, CancellationToken cancellationToken = default)
	{
		var entity = await context.Users
			.Where(u => u.Id == id)
			.SingleAsync(cancellationToken)
			.ConfigureAwait(false);

		entity.Password = hashedPassword;

		context.Users.Update(entity);

		await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}
}
