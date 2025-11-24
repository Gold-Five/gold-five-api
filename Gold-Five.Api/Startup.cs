using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Gold_Five.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Options;
using Gold_Five.Api.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Gold_Five.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors(Options =>
            {
                Options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddDbContext<StoreContext>(options =>
                options.UseSqlite("Data Source=../Registrar.sqlite",
                    b => b.MigrationsAssembly("Gold-Five.Api")));

            string authority = Configuration["Auth0:Authority"] ??
            throw new ArgumentNullException("Auth0:Authority");
            string audience = Configuration["Auth0:Audience"] ??
            throw new ArgumentNullException("Auth0:Audience");

            services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            
            .AddJwtBearer(Options =>
            {
                Options.Authority = authority;
                Options.Audience = audience;
            });
            services.AddAuthorization(Options =>
            {
                Options.AddPolicy("delete:catalog", policy =>
                    policy.RequireAuthenticatedUser()
                          .RequireClaim("scope", "delete:catalog"));
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gold_Five.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

