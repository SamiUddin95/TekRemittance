using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using System.IdentityModel.Tokens.Jwt;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Implementations;
using TekRemittance.Service.Interfaces;
using TekRemittance.Service.Implementations;
using TekRemittance.Service.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableDetailedErrors()
           .EnableSensitiveDataLogging());
builder.Services.AddHttpContextAccessor();

// Dependency Injection
builder.Services.AddScoped<IBasicSetupRepository, BasicSetupRepository>();
builder.Services.AddScoped<IBasicSetupService, BasicSetupService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenRevocationRepository, TokenRevocationRepository>();
builder.Services.AddScoped<IAcquisitionAgentsRepository, AcquisitionAgentsRepository>();
builder.Services.AddScoped<IAcquisitionAgentsService, AcquisitionAgentsService>();
builder.Services.AddScoped<IAcquisitionAgentAccountRepository, AcquisitionAgentAccountRepository>();
builder.Services.AddScoped<IAcquisitionAgentAccountService, AcquisitionAgentAccountService>();
builder.Services.AddScoped<IAgentFileTemplateRepository, AgentFileTemplateRepository>();
builder.Services.AddScoped<IAgentFileTemplateService, AgentFileTemplateService>();
builder.Services.AddScoped<IAgentFileTemplateFieldRepository, AgentFileTemplateFieldRepository>();
builder.Services.AddScoped<IAgentFileTemplateFieldService, AgentFileTemplateFieldService>();
builder.Services.AddScoped<IRemittanceInfoRepository, RemittanceInfoRepository>();
builder.Services.AddScoped<IRemittanceIngestionService, RemittanceIngestionService>();
builder.Services.AddScoped<IBranchesRepository,BranchesRepository>();
builder.Services.AddScoped<IBranchesService,BranchesService>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IDisbursementRepository, DisbursementRepository>();
builder.Services.AddScoped<IDisbursementService, DisbursementService>();
builder.Services.AddScoped<IPasswordPolicyRepository, PasswordPolicyRepository>();
builder.Services.AddScoped<IPasswordPolicyService, PasswordPolicyService>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IPermissionHelperService, PermissionHelperService>();

builder.Services.Configure<SsrsOptions>(builder.Configuration.GetSection("Ssrs"));
builder.Services.AddHttpClient<ISsrsRenderService, SsrsRenderService>("Ssrs")
    .ConfigurePrimaryHttpMessageHandler(sp =>
    {
        var opts = sp.GetRequiredService<IOptions<SsrsOptions>>().Value;
        var handler = new HttpClientHandler();
        if (opts.UseWindowsAuth)
        {
            if (!string.IsNullOrWhiteSpace(opts.Username))
            {
                handler.Credentials = new NetworkCredential(opts.Username, opts.Password, opts.Domain);
            }
            else
            {
                handler.UseDefaultCredentials = true;
            }
        }
        return handler;
    });

// Swagger & Controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Serialize enums as strings in JSON
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TekRemittance API", Version = "v1" });
    // Show TimeSpan as string in Swagger with an example
    options.MapType<TimeSpan>(() => new OpenApiSchema
    {
        Type = "string",
        Example = new OpenApiString("09:00:00"),
        Description = "Time in HH:mm:ss"
    });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter only the JWT token. The 'Bearer ' prefix is added automatically.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// JWT Authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = jwtSection.GetValue<string>("Key");
var issuer = jwtSection.GetValue<string>("Issuer");
var audience = jwtSection.GetValue<string>("Audience");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {   
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? string.Empty)),
        ClockSkew = TimeSpan.FromSeconds(5)
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrEmpty(jti))
            {
                context.Fail("Missing jti");
                return;
            }
            var repo = context.HttpContext.RequestServices.GetRequiredService<ITokenRevocationRepository>();
            if (await repo.IsRevokedAsync(jti))
            {
                context.Fail("Token revoked");
            }
        }
    };
});
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
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


