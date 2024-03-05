using Microsoft.EntityFrameworkCore;
using PlugAndPlay.WebAPI.Domain.Interfaces;
using PlugAndPlay.WebAPI.Repositories;
using PlugAndPlay.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("PlugAndPlayContext")));

builder.Services.AddScoped<ISchemaService, SchemaService>();
builder.Services.AddScoped<ISchemaRepository, SchemaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();