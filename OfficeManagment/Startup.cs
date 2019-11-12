using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficeManagment.Filters;
using OfficeManagment.Infrastructure;
using OfficeManagment.Models;
using Microsoft.EntityFrameworkCore;
using OfficeManagment.Services;
using AutoMapper;

namespace OfficeManagment
{
    public class Startup
    {

        private readonly int? _httpsPort;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            //Get the HTTPS Port ( Only in development )
            if (env.IsDevelopment())
            {
                var launchJsonConfig = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("Properties\\launchSettings.json")
                    .Build();
                _httpsPort = launchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc( opt =>
                {
                    opt.Filters.Add(typeof(JsonExceptionFilter));
                    opt.Filters.Add(typeof(LinkRewritingFilter));

                    //Require HTTPS for all Controlllers
                    opt.SslPort = _httpsPort;
                    opt.Filters.Add(typeof(RequireHttpsAttribute));

                    var jsonFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single();
                    opt.OutputFormatters.Remove(jsonFormatter);
                    opt.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));
                } 
                ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddApiVersioning(opt =>
            {
                opt.ApiVersionReader = new MediaTypeApiVersionReader();
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opt);
            });

            //Get Office data from appsettings Info Section and put it on the service container
            services.Configure<OfficeInfo>(Configuration.GetSection("Info"));

            //Use an in-memory database for quick development and testing
            //TODO : Swap out with a real database in production
            services.AddDbContext<OfficeApiContext>(opt => opt.UseInMemoryDatabase());

            // Add this line to make a scope for every incoming request as a Default Service
            services.AddScoped<IRoomService, DefaultRoomService>();

            //Add AutoMapper to the Project
            services.AddAutoMapper(typeof(MappingProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //Add Some test data for Development Purpose
                var context = serviceProvider.GetRequiredService<OfficeApiContext>();
                AddTestData(context);
            }

            //2019/11/05  Add Hsts to the project
            app.UseHsts(opt => 
            {
                opt.MaxAge(days: 365);
                opt.IncludeSubdomains();
                opt.Preload();
            });

            app.UseMvc();
        }

        private static void AddTestData(OfficeApiContext context)
        {
            context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("3454e193-4b29-445f-9fa0-d50e824af3fe"),
                Name = "Dr Noushin Mousavi Room",
                Rate = 200000
            });

            context.SaveChanges();
        }
    }
}
