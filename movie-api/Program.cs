using movie_api.ApiLayer.EndpointsCommon;
using movie_api.ApiLayer.Infrastructure;
using movie_api.InfrastructureLayer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder => builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()));


// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHealthChecks("/health");
app.UseHttpsRedirection();

app.UseExceptionHandler(options => { });
app.UseCustomExceptionHandler();
app.MapEndpoints();
app.UseEndpointDefinitions();
app.UseCors();
//app.UseAuthentication();
app.UseAuthorization();


app.Run();

public partial class Program { }
