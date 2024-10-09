var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("password", secret: true);

var mysql = builder.AddMySql("mysql");
var mysqldb = mysql.AddDatabase("mysqldb");

builder.AddProject<Projects.Aspire_MySQLSample_RESTful>("aspire-mysqlsample-restful")
	 .WithReference(mysqldb);

builder.Build().Run();
