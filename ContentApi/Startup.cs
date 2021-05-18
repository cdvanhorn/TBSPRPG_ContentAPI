using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentApi.Adapters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

using TbspRpgLib;
using TbspRpgLib.Aggregates;

using ContentApi.Repositories;
using ContentApi.Services;
using ContentApi.EventProcessors;

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

            services.AddScoped<IContentRepository, ContentRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<ISourceRepository, SourceRepository>();
            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<INewGameEventHandler, NewGameEventHandler>();
            services.AddScoped<ILocationEnterPassHandler, LocationEnterPassHandler>();
            services.AddScoped<ILocationEnterFailHandler, LocationEnterFailHandler>();
            services.AddScoped<IAggregateService, AggregateService>();

            //start workers
            services.AddHostedService<EventProcessor>();
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
