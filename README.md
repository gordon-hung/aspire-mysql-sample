# aspire-mysql-sample
ASP.NET Core 8.0 Aspire MySQL

In this article, you learn how to use the The .NET Aspire Pomelo MySQL Entity Framework Core integration. The Aspire.Pomelo.EntityFrameworkCore.MySql library is used to register a System.Data.Entity.DbContext as a singleton in the DI container for connecting to MySQL databases. It also enables connection pooling, retries, health checks, logging and telemetry.

## Get started
You need a MySQL database and connection string for accessing the database. To get started with the The .NET Aspire Pomelo MySQL Entity Framework Core integration, install the Aspire.Pomelo.EntityFrameworkCore.MySql NuGet package in the consuming client project.
```sh
dotnet add package Aspire.Pomelo.EntityFrameworkCore.MySql
```

## App host usage
To model the MySql resource in the app host, install the Aspire.Hosting.MySql NuGet package in the app host project.
```sh
dotnet add package Aspire.Hosting.MySql
```

## Migrations Overview

Create your first migration
```sh
dotnet ef migrations add InitialCreate
```

Create your database and schema
```sh
dotnet ef database update
```
