using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Aspire.MySQLSample.IntegrationTest;

internal class AppRESTfulApplication(Action<IWebHostBuilder>? webHostConfigure = null) : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		_ = builder.ConfigureAppConfiguration(c => c.Sources.Clear());
		webHostConfigure?.Invoke(builder);
	}
}
