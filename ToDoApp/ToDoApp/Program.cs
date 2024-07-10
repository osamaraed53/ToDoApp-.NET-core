using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ToDoApp.Api.Middlewares;
using ToDoApp.Core;
using ToDoApp.Core.Interfaces;
using ToDoApp.Core.Jwt;
using ToDoApp.EF.Data;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

//Jwt configuration

var jwtOptions = config.GetSection("Jwt").Get<JwtOptions>();
builder.Services.AddSingleton(jwtOptions);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.SaveToken = true;
     options.TokenValidationParameters = new TokenValidationParameters
     {   ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtOptions?.Issuer,
         ValidAudience = jwtOptions?.Audience,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
     };


 });

// Configure the default authorization policy
builder.Services.AddAuthorizationBuilder()
    .SetDefaultPolicy(new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build());

builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseSqlServer(config.GetConnectionString("DbConnection"), b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
});

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<JwtSecurityTokenHandler>();

// Add services to the container.
builder.Services.AddControllers(options => options.Filters.Add(new ExceptionHandlerFilters()));

// Register Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoApp API", Version = "v1" });
    // Add JWT token authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' following by space and JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();
EnsureCreated(app);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API V1");
    });
    app.UseDeveloperExceptionPage();
}


app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

static void EnsureCreated(WebApplication app)
{
    var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>()!;
    context.Database.EnsureCreated();

    foreach (var user in context.Users)
    {
        Console.WriteLine(user.Username);
    }
}
