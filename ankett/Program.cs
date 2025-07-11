
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace ankett
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Kestrel yapılandırması (backend portu)
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5295); // Vue frontend'in erişeceği backend portu
            });

            // Veritabanı bağlantı string'ini ayarlıyoruz
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // DbContext'i servis olarak ekliyoruz
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // MVC yapılandırması
            builder.Services.AddControllers();

            // Swagger ayarları (API belgeleme)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // CORS ayarları
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowVueApp", policy =>
                {
                    policy.WithOrigins("http://localhost:63020") //  Vue app adresi
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // JWT, cookie vs. izin ver
                });
            });
            

            // JWT Authentication yapılandırması
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false; // Geliştirme ortamında HTTPS gerekmez
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],  // Issuer'ı konfigürasyon dosyasından alıyoruz
                        ValidAudience = builder.Configuration["Jwt:Audience"],  // Audience'ı konfigürasyon dosyasından alıyoruz
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))  // Anahtarınızı konfigürasyon dosyasından alıyoruz
                    };
                });

            // Authorization yapılandırması
            builder.Services.AddAuthorization();

            // MemoryCache ve Rate Limiting servisleri  API Rate Limiting (API İstek Sınırlaması)
            builder.Services.AddMemoryCache();
            builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();


            var app = builder.Build();

            app.UseCors("AllowVueApp");


            // Rate Limiting middleware'i ekle
            app.UseIpRateLimiting();

           

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Middleware
            app.UseMiddleware<ExceptionMiddleware>();

            // HTTPS yönlendirmesi
            app.UseHttpsRedirection();

            // Authentication ve Authorization middleware'lerini doğru sırayla ekliyoruz
            app.UseAuthentication();  // Öncelikle Authentication
            app.UseAuthorization();   // Sonrasında Authorization

            // Controller'ları yönlendiriyoruz
            app.MapControllers();

            // Uygulamayı başlatıyoruz
            app.Run();
        }
    }
}
