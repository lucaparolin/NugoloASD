using AISSURE.Pilot.Data;
using AISSURE.Pilot.Services.Interfaces;
using AISSURE.Pilot.Services.Implementations;
using AISSURE.Pilot.Repositories.Interfaces;
using AISSURE.Pilot.Repositories.Implementations;
using AISSURE.Pilot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/aissure-pilot-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add API Controllers
builder.Services.AddControllers();

// Add HttpContextAccessor for multi-tenant support
builder.Services.AddHttpContextAccessor();

// Configure Database Connection
builder.Services.AddSingleton<IDbConnectionFactory>(sp =>
    new SqlConnectionFactory(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is missing");

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Configure Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Amministratore", policy => policy.RequireRole("Amministratore"));
    options.AddPolicy("Segretario", policy => policy.RequireRole("Amministratore", "Segretario"));
    options.AddPolicy("Istruttore", policy => policy.RequireRole("Amministratore", "Segretario", "Istruttore"));
    options.AddPolicy("Tesserato", policy => policy.RequireRole("Amministratore", "Segretario", "Istruttore", "Tesserato"));
});

// Register Repositories
builder.Services.AddScoped<IAssociazioneRepository, AssociazioneRepository>();
builder.Services.AddScoped<IUtenteRepository, UtenteRepository>();
builder.Services.AddScoped<ISocioRepository, SocioRepository>();
builder.Services.AddScoped<ICorsoRepository, CorsoRepository>();
builder.Services.AddScoped<IImpiantoRepository, ImpiantoRepository>();
builder.Services.AddScoped<ITesseramentoRepository, TesseramentoRepository>();
builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();

// Register Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ISocioService, SocioService>();
builder.Services.AddScoped<ICorsoService, CorsoService>();
builder.Services.AddScoped<IImpiantoService, ImpiantoService>();
builder.Services.AddScoped<ITesseramentoService, TesseramentoService>();
builder.Services.AddScoped<IPagamentoService, PagamentoService>();
builder.Services.AddScoped<IComunicazioneService, ComunicazioneService>();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AISSURE Pilot API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

// Use Tenant Middleware for multi-tenant support
app.UseTenantMiddleware();

app.UseAuthentication();
app.UseAuthorization();

// Map MVC Controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map API Controllers
app.MapControllers();

app.Run();
