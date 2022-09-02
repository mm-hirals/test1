using MidCapERP.WebAPI.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureBuilder();

builder.Build().ConfigureWebApplication();

// Add services to the container.
//builder.Services.AddControllersWithViews();

//Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
//builder.Host.UseSerilog(((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration)));
//builder.Services.AddSwaggerGen();
//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}
//app.UseSwagger();
//app.UseSwaggerUI();

//app.UseSerilogRequestLogging();
//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();