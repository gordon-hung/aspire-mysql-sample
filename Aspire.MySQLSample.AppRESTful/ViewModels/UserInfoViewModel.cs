using Aspire.MySQLSample.Core.Enums;

namespace Aspire.MySQLSample.AppRESTful.ViewModels;

public record UserInfoViewModel(
	string Id,
	string Username,
	UserState State,
	DateTimeOffset CreatedAt,
	DateTimeOffset UpdateAt);
