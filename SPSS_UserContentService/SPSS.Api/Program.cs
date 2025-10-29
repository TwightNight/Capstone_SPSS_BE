using Serilog; // Thêm Serilog
using SPSS.Api;
using SPSS.Api.Middlewares;
using System;

// --- CẤU HÌNH SERILOG (BẮT ĐẦU) ---
// Đọc cấu hình từ appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(WebApplication.CreateBuilder(args).Configuration) // Đọc config mồi
    .CreateBootstrapLogger();

try
{
    Log.Information("Đang khởi tạo ứng dụng...");

    var builder = WebApplication.CreateBuilder(args);

    // B ảo .NET dùng Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // --- VÙNG ĐĂNG KÝ DI ---
    builder.Services.AddApplicationServices();
    builder.Services.AddPersistenceServices(builder.Configuration);
    builder.Services.AddPresentationServices(builder.Configuration);

    // KHÔNG CẦN DÒNG NÀY NỮA (vì đã có trong AddPresentationServices)
    // builder.Services.AddOpenApi(); 
    // ---------------------------------

    var app = builder.Build();

    // Thêm Middleware ghi log request của Serilog
    app.UseSerilogRequestLogging();

    // --- CẤU HÌNH HTTP PIPELINE ---
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
	app.UseMiddleware<ExceptionMiddleware>();
    app.UseMiddleware<AuthMiddleware>();
	app.UseCors("AllowAll"); // Đảm bảo "AllowAll" đã được định nghĩa trong AddPresentationServices
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // --- CHẠY APP ---
    Log.Information("Khởi động dịch vụ thành công.");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Dịch vụ khởi động thất bại!");
}
finally
{
    Log.CloseAndFlush(); // Đảm bảo mọi log đều được ghi xuống file trước khi tắt
}