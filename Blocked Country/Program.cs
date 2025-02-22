
using Blocked_CountryCore.Interfaces;
using Blocked_CountryServices.Services;

namespace Blocked_Country
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient();
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<IIpApiService, IpApiService>();
            builder.Services.AddHostedService<BackGroundServices>();
            builder.Services.AddScoped<Ilogservices, LogServices>();
            builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddSingleton<ICachingServices, CachingServices>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
