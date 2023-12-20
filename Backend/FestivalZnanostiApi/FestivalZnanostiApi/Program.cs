using DinkToPdf;
using DinkToPdf.Contracts;
using FestivalZnanostiApi.Middlewares;
using FestivalZnanostiApi.Middlewares.UserContext;
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
using FluentValidation.AspNetCore;
using FestivalZnanostiApi.DTOs.Validators;

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


builder.Services.AddCors(c =>
{
    c.AddPolicy("CorsPolicy", options =>
        options.AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(
                "http://localhost:7282",
                "http://localhost:3000", 
                "https://4ivliy722g3exbyg.myfritz.net:7282",
                "https://festival-znanosti-zagreb.vercel.app")
    // .AllowAnyOrigin()
    );
});


builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddSingleton<UserContext>(); // Register UserContext as a singleton


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddDbContext<FestivalZnanostiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FestivalZnanosti")),
    ServiceLifetime.Transient);

// Services
builder.Services.AddTransient<IFestivalYearService, FestivalYearService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ITimeSlotService, TimeSlotService>();
builder.Services.AddTransient<IEventsService, EventsService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IFilesService, FilesService>();
builder.Services.AddTransient<ILocationService, LocationService>();

// Validators
builder.Services.AddScoped<CreateFestivalYearDtoValidator>();



// Repositories
builder.Services.AddTransient<IFestivalYearRepository, FestivalYearRepository>();
builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<ITimeSlotRepository, TimeSlotRepository>();
builder.Services.AddTransient<IEventsRepository, EventsRepository>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<ILocationRepository, LocationRepository>();


var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();


// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy();

app.UseMiddleware<UserContextMiddleware>();

app.MapControllers();

app.Run();
