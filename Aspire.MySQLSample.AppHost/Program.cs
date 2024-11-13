var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql");
var mysqldb = mysql.AddDatabase("mysqldb");

var migration = builder.AddProject<Projects.Aspire_MySQLSample_AppDataMigration>("aspire-mysqlsample-app-data-migration")
	.WithReference(mysqldb);

var seeding = builder.AddProject<Projects.Aspire_MySQLSample_AppDataSeeding>("aspire-mysqlsample-app-data-seeding")
	.WithReference(mysqldb)
	.WithEnvironment("migration-api", migration.GetEndpoint("https"))
	.WithExternalHttpEndpoints();

builder.AddProject<Projects.Aspire_MySQLSample_AppRESTful>("aspire-mysqlsample-app-restful")
	.WithReference(mysqldb)
	.WithEnvironment("seeding-api", seeding.GetEndpoint("https"))
	.WithExternalHttpEndpoints();

builder.Build().Run();
