﻿using MediatR;
using Aspire.MySQLSample.Core;
using Aspire.MySQLSample.Core.ApplicationServices;

namespace Aspire.MySQLSample.Core.ApplicationServices;

internal class UserAddRequestHandler(
	IUserIdGenerator generator,
	IPasswordHasher hasher,
	IUserRepository repository) : IRequestHandler<UserAddRequest, string>
{
	public async Task<string> Handle(UserAddRequest request, CancellationToken cancellationToken)
	{
		var id = generator.NewId();
		var hashedPassword = hasher.HashPassword(request.Password);

		await repository.AddAsync(
			id: id,
			username: request.Username,
			hashedPassword: hashedPassword,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return id;
	}
}
