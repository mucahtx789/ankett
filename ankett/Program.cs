using Microsoft.EntityFrameworkCore;
using System;

namespace ankett
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5295); // Vue frontend'in eriþeceði backend portu
            });

            /* // SQL Server baðlantýsý
             builder.Services.AddDbContext<ApplicationDbContext>(opts =>
             {

                  opts.UseSqlServer("Server=DESKTOP-VI5LI79;Database=Ankett;User Id=sa;Password=123;Encrypt=False;TrustServerCertificate=True;");

             });*/
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // DbContext'i servis olarak ekliyoruz
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));


            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // CORS ayarlarý 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowVueApp", policy =>
                {
                    policy.WithOrigins("http://localhost:63020") //  Vue app adresi
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); //  (JWT, cookie vs.)
                });
            });

            var app = builder.Build();

            // CORS middleware’i mutlaka en üstte çalýþmalý
            app.UseCors("AllowVueApp");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
