using System;
using ContentApi.Repositories;
using ContentApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TbspRpgLib;
using TbspRpgLib.Aggregates;

namespace ContentApi
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
            services.AddControllers();
            LibStartup.ConfigureTbspRpgServices(Configuration, services);

            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            services.AddDbContext<ContentContext>(
                options => options.UseNpgsql(connectionString)
            );

            services.AddScoped<ISourceRepository, SourceRepository>();
            services.AddScoped<IConditionalSourceRepository, ConditionalSourceRepository>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<IAggregateService, AggregateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            LibStartup.ConfigureTbspRpg(app);

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
