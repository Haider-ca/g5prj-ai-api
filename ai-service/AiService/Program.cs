// Author: Haider Al-Sudani
// Course: COMP 4537
// Note: ChatGPT was used as a learning tool.

using AiService.Clients;
using AiService.Interfaces.Clients;
using AiService.Interfaces.Services;
using AiService.Middleware;
using AiService.Options;
using AiService.Services;
using AiService.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AuthServiceOptions>(
    builder.Configuration.GetSection(AuthServiceOptions.SectionName));

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.SectionName));

builder.Services.Configure<CorsOptions>(
    builder.Configuration.GetSection(CorsOptions.SectionName));

builder.Services.AddScoped<IAiEvaluationService, AiEvaluationService>();
builder.Services.AddScoped<EvaluateRequestValidator>();
builder.Services.AddHttpClient<IAuthUsageClient, AuthUsageClient>();
builder.Services.AddScoped<IAiProviderClient, AiProviderClient>();

var jwtOptions = builder.Configuration
    .GetSection(JwtOptions.SectionName)
    .Get<JwtOptions>() ?? new JwtOptions();

if (string.IsNullOrWhiteSpace(jwtOptions.Key))
{
    throw new InvalidOperationException("JWT key is missing.");
}

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key + "\n"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = !string.IsNullOrWhiteSpace(jwtOptions.Issuer),
            ValidateAudience = !string.IsNullOrWhiteSpace(jwtOptions.Audience),
            ValidateLifetime = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

var corsOptions = builder.Configuration
    .GetSection(CorsOptions.SectionName)
    .Get<CorsOptions>() ?? new CorsOptions();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        if (corsOptions.AllowedOrigins != null && corsOptions.AllowedOrigins.Count > 0)
        {
            policy.WithOrigins(corsOptions.AllowedOrigins.ToArray())
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();