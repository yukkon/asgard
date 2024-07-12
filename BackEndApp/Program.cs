using BackEndApp.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

if (builder.Configuration != null)
{
    builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
}

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

//builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddSingleton(builder.Environment);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseCors(x => x
  .SetIsOriginAllowed(origin => true)
  .AllowAnyMethod()
  .AllowAnyHeader()
  .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        //var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        //var exception = exceptionHandlerPathFeature.Error;

        var result = new ContentResult()
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            ContentType = Application.Json,
            Content = "An unexpected error occurred"
        };

        await result.ExecuteResultAsync(new ActionContext
        {
            HttpContext = context
        });
    });
});

app.MapFallbackToFile("index.html");

app.Run();
