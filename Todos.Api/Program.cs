using Todos.Api.Repositories;
using Todos.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddTodoServices();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


    builder.Services.UploadTodoData();
    builder.Services.UploadUserData();
    // faq: возможна ли така€ реализаци€ загрузки данных?
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
