using Microsoft.EntityFrameworkCore;
using FluentValidation;
using MediatR;

using Igloo.Infrastructure.Persistence;
using Igloo.Infrastructure.Services;
using Igloo.UseCases.Users.CreateUser;
using Igloo.Middlewares;
using Igloo.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IglooDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(CreateUserCommandValidator).Assembly);
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorHandling();
app.UseJwtAuthentication();
app.MapControllers();
app.UseHttpsRedirection();
await app.RunAsync();
