
using System.Reflection;
using System.Text.Json;
using CalculadoraCDB.Infraestrutura;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5000");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
    
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

InjecaoDependencia.ConfigurarDependencias(builder.Services, builder.Configuration);

var app = builder.Build();

app.UseCors("AllowAngular");

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        if (exception != null)
        {
            logger.LogError(exception, "Erro não tratado na aplicação.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new { error = "Ocorreu um erro ao processar a solicitação. Por favor, tente novamente." });
            await context.Response.WriteAsync(result);
        }
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.MapControllers();

app.Run();