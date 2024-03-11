using Common.Api;
using Common.Repositiories;
using Serilog;
using Serilog.Events;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text.Json.Serialization;
using Todos.Api.Repositories;
using Todos.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.File("Logs/information-.txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
    .WriteTo.File("Logs/errors-.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers()
        .AddJsonOptions(t => t.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();


    builder.Services.AddTodoServices();

    builder.Services.AddSwaggerGen();

    builder.Services.AddFluentValidationAutoValidation();

    builder.Host.UseSerilog();

    builder.Services.AddTodoDatabase(builder.Configuration);

    var app = builder.Build();

    app.UseExceptionsHandler();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();


        builder.Services.UploadUserData();
        builder.Services.UploadTodoData();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Appliation running ...");
    app.Run();

}
catch (Exception e)
{
    Log.Error(e, "Run error");
}
