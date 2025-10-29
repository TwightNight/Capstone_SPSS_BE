using Serilog;
using SPSS.Api;
using SPSS.Api.Middlewares;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(WebApplication.CreateBuilder(args).Configuration)
    .CreateBootstrapLogger();

try
{
    Log.Information("Initializing the application...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddApplicationServices();
    builder.Services.AddPersistenceServices(builder.Configuration);
    builder.Services.AddPresentationServices(builder.Configuration);

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseMiddleware<ExceptionMiddleware>();

    app.UseCors("AllowAll");

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Service started successfully.");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Service failed to start!");
}
finally
{
    Log.CloseAndFlush();
}