using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using auth.server.Repositories;
using auth.server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MySqlConnector;

namespace auth.server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // STUB[epic=Auth] copy/paste
            services.AddAuthentication(options =>
              {
                  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              }).AddJwtBearer(options =>
          {
              options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
              options.Audience = Configuration["Auth0:Audience"];
          });

            services.AddCors(options =>
           {
               options.AddPolicy("CorsDevPolicy", builder =>
           {
               builder
                     .WithOrigins(new string[]{
                              "http://localhost:8080",
                                "http://localhost:8081"
                           })
                  .AllowAnyMethod()
                  .AllowAnyHeader()
            .AllowCredentials();
           });
           });

            services.AddScoped<AccountsService>();

            services.AddScoped<AccountsRepository>();

            services.AddScoped<IDbConnection>(x => CreateDbConnection());
            // end copy/paste



            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "auth.server", Version = "v1" });
            });
        }



        // STUB[epic=DB] database Connection
        private IDbConnection CreateDbConnection()
        {
            string connectionString = Configuration["DB:gearhost"];
            return new MySqlConnection(connectionString);
        }
        // end copy/paste



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "auth.server v1"));



                // STUB[epic=Auth] Invoke your Cors
                app.UseCors("CorsDevPolicy");
                // end copy/paste



            }

            app.UseHttpsRedirection();



            // STUB Add Default and Static Files
            app.UseDefaultFiles();
            app.UseStaticFiles();
            // end copy/paste



            app.UseRouting();



            // STUB[epic=Auth] Add Authentication so bearer gets validated
            app.UseAuthentication();
            // end copy/paste



            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
