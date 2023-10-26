using DinkToPdf;
using DinkToPdf.Contracts;
using FestivalZnanostiApi.Middlewares;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Repositories;
using FestivalZnanostiApi.Repositories.impl;
using FestivalZnanostiApi.Services;
using FestivalZnanostiApi.Services.impl;
using FestivalZnanostiApi.Servicess;
using FestivalZnanostiApi.Servicess.impl;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "FestivalZnanostiCookie";
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                // User is not authenticated, return 401 Unauthorized
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                // User is authenticated but lacks necessary permissions, return 403 Forbidden
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
        };
    });



builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FestivalZnanostiContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FestivalZnanosti")));

// Services
builder.Services.AddTransient<IFestivalYearService, FestivalYearService>();
//builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ITimeSlotService, TimeSlotService>();
builder.Services.AddTransient<IEventsService, EventsService>();
builder.Services.AddTransient<ISubmitterService, SubmitterService>();
builder.Services.AddTransient<IFilesService, FilesService>();




// Repositories
builder.Services.AddTransient<IFestivalYearRepository, FestivalYearRepository>();
//builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<ITimeSlotRepository, TimeSlotRepository>();
builder.Services.AddTransient<IEventsRepository, EventsRepository>();
builder.Services.AddTransient<ISubmitterRepository, SubmitterRepository>();


var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy();

app.MapControllers();

app.Run();
