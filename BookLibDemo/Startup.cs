using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibService.Models;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookLibService
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
            services.AddOData();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });
            services.AddSingleton<BookLibDbContext>(_ => new BookLibDbContext());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var db = app.ApplicationServices.GetRequiredService<BookLibDbContext>();
            DataSeeder.SeedDatabase(db);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseODataBatching();
            app.UseMvc(routeBuilder =>
            {
                var odataBatchHandler = new DefaultODataBatchHandler();
                routeBuilder.Select().Filter().Expand().MaxTop(100).OrderBy().Count();
                routeBuilder.MapODataServiceRoute("ODataRoute", "odata", BookLibEdmModel.GetEdmModel(), odataBatchHandler);
            });
        }
    }
}
