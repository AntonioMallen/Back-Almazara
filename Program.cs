using Asp.Versioning;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Back_Almazara.DTOS;
using FluentValidation;
using ProyectoJWT.Validator;
using ProyectoJWT.Mapper;
using Back_Almazara.Service.V1;
using Back_Almazara.Repository.V1;
using Back_Almazara.Utility;
using static Back_Almazara.Utility.TypeConverter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettingsDTO>(jwtSettingsSection);
builder.Services.AddControllers();

//  Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<INoticeService, NoticeService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<StringToByteArrayConverter>();
builder.Services.AddScoped<ByteArrayToStringConverter>();

//  Entities
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<INoticesRepository, NoticesRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();


builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperApp>());
builder.Services.AddScoped<IHashUtility, HashUtility>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MiPoliticaCORS", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://192.168.1.15:3000", "https://almazara.ssbros.es", "https://almazara.mallengimeno.dev") // Origen EXPLÍCITO
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials() // Solo si usas cookies o auth headers
              .WithExposedHeaders("WWW-Authenticate");
    });
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.ReportApiVersions = true;
})
    .AddApiExplorer(options => // Método correcto
{
    options.GroupNameFormat = "'v'VVV"; // Formato: v1, v2, etc.
    options.SubstituteApiVersionInUrl = true; // ¡Clave para reemplazar {version}!
});


builder.Services.AddEndpointsApiExplorer();


var jwtSettings = jwtSettingsSection.Get<JwtSettingsDTO>()
                  ?? throw new InvalidOperationException("La configuración de JWT es requerida.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(jwtSettings.SecretKey))
    };
});
builder.Services.AddSwaggerGen(options =>
{
    //Token
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Eusuarioample: \"Bearer {token}\"",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    var securityRequirement = new OpenApiSecurityRequirement
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
                        new List<string>()
                    }
                };
    options.AddSecurityRequirement(securityRequirement);


    //Versioning
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

app.UseRouting();
app.UseCors("MiPoliticaCORS");

//app.UseMiddleware<TokenValidationMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
