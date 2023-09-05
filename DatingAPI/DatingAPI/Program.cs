using DatingAPI.Data;
using DatingAPI.Filters;
using DatingAPI.Helpers;
using DatingAPI.Interfaces;
using DatingAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Globalization;

const string ORIGIN_NAME = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
ConfigureCorsPolicy(builder);

ConfigureDIServices(builder);

builder.Services.AddMvc();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Dating API" });

    setup.CustomSchemaIds(x => x.FullName);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(ORIGIN_NAME);
}

app.UseRequestLocalization(options =>
{
    var supportedCultures = new List<CultureInfo> { new CultureInfo("hr") };
    options.DefaultRequestCulture = new RequestCulture("hr");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureCorsPolicy(WebApplicationBuilder appbuilder)
{
    var origin = appbuilder.Configuration["FrontendApp"] ?? "http://localhost:4200";

    appbuilder.Services.AddCors(options =>
    {
        options.AddPolicy(name: ORIGIN_NAME,
            builder =>
            {
                builder.WithOrigins(origin).SetIsOriginAllowed(origin => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders("Content-Disposition");
            });
    });
}

void ConfigureDIServices(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IJwtHelper, JwtHelper>();
    builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection(nameof(TokenOptions)));
    builder.Services.AddDbContext<DatingContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("Connectionstring"))
    );

    builder.Services.AddControllers(c =>
        c.Filters.Add<GlobalExceptionFilter>()
    );
}