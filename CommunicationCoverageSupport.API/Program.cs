// File: CommunicationCoverageSupport/Program.cs
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
using Microsoft.OpenApi.Models;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Database, SSH, etc.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.Configure<SshSettings>(builder.Configuration.GetSection("SshSettings"));

// SSH Client
builder.Services.AddScoped<ISshHlrClient, SshHlrClient>();

// Auth
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Other repositories & services...
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

// Read JWT settings from "Jwt" section of appsettings.json
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];
var keyBytes = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

// Configure Authentication & JWT Bearer
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

// Configure Swagger/OpenAPI with JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid JWT token."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
