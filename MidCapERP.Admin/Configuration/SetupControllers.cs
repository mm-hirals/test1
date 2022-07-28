﻿using MidCapERP.Admin.Middleware;

namespace MidCapERP.Admin.Configuration
{
    /// <summary>
    ///     Configure MVC options
    /// </summary>
    public static class MvcConfig
    {
        /// <summary>
        ///     Configure controllers
        /// </summary>
        /// <param name="services"></param>
        public static void SetupControllers(this IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<CurrentUserActionFilter>();
            services.AddMvc(options =>
            {
                options.Filters.AddService<CurrentUserActionFilter>();
            });

            // API controllers
            //services.AddControllers(
            //            //options =>
            //            // handle exceptions thrown by an action
            //            //options.Filters.Add(new ApiExceptionFilterAttribute())
            //            )
            //        //.AddNewtonsoftJson(options =>
            //        //{
            //        //    // Serilize enum in string
            //        //    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            //        //})
            //        //.AddFluentValidation(options =>
            //        //{
            //        //    // In order to register FluentValidation to define Swagger schema
            //        //    // https://github.com/RicoSuter/NSwag/issues/1722#issuecomment-544202504
            //        //    // https://github.com/zymlabs/nswag-fluentvalidation
            //        //    options.RegisterValidatorsFromAssemblyContaining<Startup>();

            //        //    // Optionally set validator factory if you have problems with scope resolve inside validators.
            //        //    options.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
            //        //})
            //        .AddMvcOptions(options =>
            //        {
            //            // Clear the default MVC model binding and model validations, as we are registering all model binding and validation using FluentValidation.
            //            // See ApiExceptionFilterAttribute.cs
            //            // https://github.com/jasontaylordev/NorthwindTraders/issues/76
            //            //options.ModelMetadataDetailsProviders.Clear();
            //            //options.ModelValidatorProviders.Clear();
            //        });
        }
    }
}