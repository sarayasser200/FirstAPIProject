
using FirstAPIProject;
using FirstAPIProject.Authentication;
using FirstAPIProject.Authorization;
using FirstAPIProject.Data;
using FirstAPIProject.Filters;
using FirstAPIProject.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// to read from custom configuration 
builder.Configuration.AddJsonFile("config.json");
builder.Services.AddLogging(cfg =>
{
    cfg.AddDebug();
});


builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection("Attachments"));
// 1 first way to configure object 
//var attachmentOptions = builder.Configuration.GetSection("Attachments").Get<AttachmentOptions>();
//builder.Services.AddSingleton(attachmentOptions);
// 2 second way to configure object
//var attachmentOptions = new AttachmentOptions();
//builder.Configuration.GetSection("Attachments").Bind(attachmentOptions);
//builder.Services.AddSingleton(attachmentOptions);
// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<logActivityFilter>();
    options.Filters.Add<PermissionBasedAuthorizationFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(cfg => cfg.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
builder.Services.AddSingleton(jwtOptions);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AgeGreaterThan25", builder => builder.AddRequirements(new AgeGreaterThan25Requirement()));
    
});
builder.Services.AddSig
builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issure,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
        };

    });
    //.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic",null);
var app = builder.Build();

// Configure the HTTP request pipeline. for development only 
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
// for all 
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ProfilingMiddleware>();

app.MapControllers();
app.UseStaticFiles();

app.Run();
