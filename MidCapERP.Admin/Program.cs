using MidCapERP.Admin.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureBuilder();

builder.Build().ConfigureWebApplication();