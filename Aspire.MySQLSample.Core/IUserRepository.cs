using Aspire.MySQLSample.Core.Models;

namespace Aspire.MySQLSample.Core;

public interface IUserRepository
{
	ValueTask AddAsync(string id, string username, string hashedPassword, CancellationToken cancellationToken = default);

	ValueTask<UserInfo?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

	ValueTask<UserInfo?> GetAsync(string id, CancellationToken cancellationToken = default);

	ValueTask UpdatePasswordAsync(string id, string hashedPassword, CancellationToken cancellationToken = default);
}
