
using HotelHostingApi.Configuration;
using HotelHostingApi.EF.Data;
using HotelLisstingApi.Core.IRepositories;
using HotelLisstingApi.Core.Models;
using HotelListingApi.EF.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace HotelHostingApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //add dbcontext setting
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //setup user identity
            builder.Services.AddIdentity<ApiUser , IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>().AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
                

            

            //services.AddIdentity<User, UserRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // add CORS 
            //named CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    p => p.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            //set api versioning
            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                     new QueryStringApiVersionReader("api-version"),
                     new HeaderApiVersionReader("X-Version"),
                     new MediaTypeApiVersionReader("ver")
                );
            });

            builder.Services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            //serilog setting
            builder.Host.UseSerilog((builder , LoggerConfiguration) => 
                LoggerConfiguration.WriteTo.Console()
                .ReadFrom.Configuration(builder.Configuration)
             );

            //AutoMapper setting
            builder.Services.AddAutoMapper(typeof(MapperConfig));

            //set repository pattern
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IAuthManager, AuthManager>();

            //jwt settings
            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                };
            });

            //Set Response Cashing
            builder.Services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = 1024;
                options.UseCaseSensitivePaths = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            //Response Cashing
            app.UseResponseCaching();

            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}