using MagnusMinds.Utility.EmailService;
using Microsoft.OpenApi.Models;
using MidCapERP.Infrastructure.ServiceDependency;
using Serilog;
using System.Reflection;

namespace MidCapERP.WebAPI.Configuration
{
    public static class BuilderMethod
    {
        public static void ConfigureBuilder(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
            builder.Host.UseSerilog(((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration)));

            ConfigurationManager configuration = builder.Configuration;
            if (string.Equals(configuration["AppSettings:EnableSwagger"], "true", StringComparison.CurrentCultureIgnoreCase))
            {
                builder.Services.AddSwaggerGen(opt =>
                {
                    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    });
                    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
                    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);
                });
            }

            builder.Services.ConfigureEmail(configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            builder.Services.AddCors();
            builder.Services.SetupControllers();
            builder.Services.SetupIdentityDatabase(configuration);
            builder.Services.SetupDIServices(configuration, "Bearer");
        }
    }
}