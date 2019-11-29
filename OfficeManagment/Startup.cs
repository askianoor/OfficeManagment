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
using Newtonsoft.Json;

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

                    //Cache Profiles
                    opt.CacheProfiles.Add("Static", new CacheProfile
                    {
                        Duration = 86400
                    });
                })
                .AddJsonOptions(opt =>
                {
                    // These should be the defaults, but we can be explicit:
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    opt.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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
            services.Configure<OfficeOptions>(Configuration);
            services.Configure<PagingOptions>(Configuration.GetSection("DefaultPagingOptions"));

            //Use an in-memory database for quick development and testing
            //TODO : Swap out with a real database in production
            services.AddDbContext<OfficeApiContext>(opt => opt.UseInMemoryDatabase());

            // Add this line to make a scope for every incoming request as a Default Service
            services.AddScoped<IRoomService, DefaultRoomService>();
            services.AddScoped<IOpeningService, DefaultOpeningService>();
            services.AddScoped<IBookingService, DefaultBookingService>();
            services.AddScoped<IDateLogicService, DefaultDateLogicService>();

            //Add AutoMapper to the Project
            services.AddAutoMapper(typeof(MappingProfile));

            //Add Server side Response Caching
            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //Add Some test data for Development Purpose
                var context = serviceProvider.GetRequiredService<OfficeApiContext>();
                //var dateLogicService = app.ApplicationServices.GetRequiredService<IDateLogicService>();
                AddTestData(context);//, dateLogicService);
            }

            //2019/11/05 Add Hsts to the project
            app.UseHsts(opt => 
            {
                opt.MaxAge(days: 365);
                opt.IncludeSubdomains();
                opt.Preload();
            });

            //Add Server side Response Caching
            app.UseResponseCaching();

            app.UseMvc();
        }

        private static void AddTestData(OfficeApiContext context)//, IDateLogicService dateLogicService)
        {
            context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("3454e193-4b29-445f-9fa0-d50e824af3fe"),
                Name = "Dr Noushin Mousavi Room",
                Rate = 200000
            });


            var oxford = context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("301df04d-8679-4b1b-ab92-0a586ae53d08"),
                Name = "Oxford Suite",
                Rate = 10119,
            }).Entity;

            var today = DateTimeOffset.Now;
            //var start = dateLogicService.AlignStartTime(today);
            //var end = start.Add(dateLogicService.GetMinimumStay());

            context.Bookings.Add(new BookingEntity
            {
                Id = Guid.Parse("2eac8dea-2749-42b3-9d21-8eb2fc0fd6bd"),
                Room = oxford,
                CreatedAt = DateTimeOffset.UtcNow,
                //StartAt = start,
                //EndAt = end,
                Total = oxford.Rate,
            });

            context.SaveChanges();
        }
    }
}
