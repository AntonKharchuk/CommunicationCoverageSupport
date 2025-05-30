using CommunicationCoverageSupport.API.Controllers;
using CommunicationCoverageSupport.BLL.Services;
using CommunicationCoverageSupport.BLL.Services.Auth;
using CommunicationCoverageSupport.BLL.Services.SimCards;
using CommunicationCoverageSupport.BLL.Services.TransportKeys;
using CommunicationCoverageSupport.DAL.Clients;
using CommunicationCoverageSupport.DAL.Repositories;
using CommunicationCoverageSupport.DAL.Repositories.Auth;
using CommunicationCoverageSupport.DAL.Repositories.SimCards;
using CommunicationCoverageSupport.DAL.Repositories.TransportKeys;
using CommunicationCoverageSupport.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.Configure<SshSettings>(builder.Configuration.GetSection("SshSettings"));



// Authentication - JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = false; // Set to true in production!
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        ClockSkew = TimeSpan.Zero
//    };
//});

// Client for SSH services
builder.Services.AddScoped<ISshHlrClient, SshHlrClient>();


// Business services


builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddScoped<ISimCardRepository, SimCardRepository>();
builder.Services.AddScoped<ISimCardService, SimCardService>();


builder.Services.AddScoped<IArtworkRepository, ArtworkRepository>();
builder.Services.AddScoped<IArtworkService, ArtworkService>();

builder.Services.AddScoped<IAccRepository, AccRepository>();
builder.Services.AddScoped<IAccService, AccService>();

builder.Services.AddScoped<ITransportKeyRepository, TransportKeyRepository>();
builder.Services.AddScoped<ITransportKeyService, TransportKeyService>();

builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IOwnerService, OwnerService>();

builder.Services.AddScoped<ISimCardDrainRepository, SimCardDrainRepository>();
builder.Services.AddScoped<ISimCardDrainService, SimCardDrainService>();


builder.Services.AddControllers();

// Swagger (for testing APIs easily)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Authentication and Authorization middleware!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
