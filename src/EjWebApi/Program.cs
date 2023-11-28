using System.Text;
using EjWebApi.DataAccess;
using EjWebApi.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<AcademiaDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("AcademiaDb")));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt => opt.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ClaveDeSeguridadConUnMínimoDe256Bits")),
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true
    });

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(opt =>
    {
        opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
        {
            In = ParameterLocation.Header,
            Name = HeaderNames.Authorization,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        });

        opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            [new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            }] = Array.Empty<string>()
        });
    });

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter());
    opt.Filters.Add<AcademiaFilter>();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.MapControllers();

app.Run();
