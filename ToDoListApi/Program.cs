using ToDoListApi.Services;
using ToDoListApi.Extensions;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDataContext(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen("v1", "Todo List API");
builder.Services.AddProblemDetails();
builder.Services.AddAuthentication("localhost", "localhost", "your-secret-key-that-must-be-at-least-16-characters-long");    
builder.Services.AddAuthorization("Bearer");
builder.Services.AddLocalization();
builder.Services.AddScoped<IStringLocalizer, StringLocalizerService>();
builder.Services.AddHealthChecks()
        .AddSqlHealthCheck(builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseRequestLocalization("uk-UA");
app.MapHealthChecks();

app.Run();

