using Aspire.MySQLSample.Core.Enums;

namespace Aspire.MySQLSample.Core.Models;
public record UserInfo(
	string Id,
	string Username,
	string Password,
	UserState State,
	DateTimeOffset CreatedAt,
	DateTimeOffset UpdateAt);
