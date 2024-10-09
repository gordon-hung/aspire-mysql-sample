namespace Aspire.MySQLSample.Core;

public interface IPasswordHasher
{
	public string HashPassword(string plainPassword);

	public bool VerifyPassword(string plainPassword, string hashedPassword);
}
