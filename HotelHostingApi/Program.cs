
using HotelHostingApi.Configuration;
using HotelHostingApi.EF.Data;
using HotelLisstingApi.Core.IRepositories;
using HotelListingApi.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

            //serilog setting
            builder.Host.UseSerilog((builder , LoggerConfiguration) => 
                LoggerConfiguration.WriteTo.Console()
                .ReadFrom.Configuration(builder.Configuration)
             );

            //AutoMapper setting
            builder.Services.AddAutoMapper(typeof(MapperConfig));

            //set repository pattern
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}