using Aspire.MySQLSample.Core.Enums;

namespace Aspire.MySQLSample.Core.ApplicationServices;
public record UserInfoResponse(
	string Id,
	string Username,
	UserState State,
	DateTimeOffset CreatedAt,
	DateTimeOffset UpdateAt);
