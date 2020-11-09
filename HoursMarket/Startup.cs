using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HoursMarket.Data;
using HoursMarket.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace HoursMarket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // Github secrets tracking prevention code, remove it and fill appsettings file and change appsettings.json action to Content

            Configuration = new ConfigurationBuilder()
         .AddJsonFile($"appsettings.secrets.json")
         .Build();


            StaticConfig = Configuration;
        }

        public static IConfiguration StaticConfig { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {





            services.AddCors();


#if DEBUG

            services.AddDbContext<HoursMarketContext>(opt => opt.UseSqlServer(Configuration["Secrets:HoursMarketConection"]));
#elif RELEASE
            services.AddDbContext<HoursMarketContext>(opt => opt.UseSqlServer(Configuration["ConnectionStrings:HoursMarketConection"]));
#endif






            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IHoursMarketRepo, SqlHoursMarketRepo>();
            services.AddScoped<IAuthenticator, JwtAuthenticator>();
            services.AddScoped<IAuthorizer, Authorizer>();
            services.AddScoped<IEmailSender, SmtpProvider>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtAuthenticator.GetValidationParameters();
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors(builder => builder
       .AllowAnyOrigin()
       .AllowAnyMethod()
       .AllowAnyHeader());


            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
