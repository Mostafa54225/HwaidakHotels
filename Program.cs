using HwaidakAPI.CheckInModels;
using HwaidakAPI.Helpers;
using HwaidakAPI.Middleware;
using HwaidakAPI.Models;
using HwaidakAPI.OPModels;
using Microsoft.EntityFrameworkCore;
using OrientHGAPI.Helpers;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<HwaidakHotelsWsdbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

builder.Services.AddDbContext<HwaidakHotelsOpedbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("OPDBConnectionString"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

builder.Services.AddDbContext<HwaidakHotelsOnlineCheckInDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("OnlineCheckInDBConnectionString"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddHttpClient();
builder.Services.AddHostedService<PeriodicRequestService>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAnyOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();
// Configure the HTTP request pipeline.

if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAnyOrigin");

app.Run();
