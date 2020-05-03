using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StatusUpdateEventAPI.Helpers;
using Associa.Service.DAL.Models;
using Associa.Service.DAL.Interfaces;
using Associa.Service.DAL.Repositories;

using Associa.Service.BAL.Interfaces;
using Associa.Service.BAL.BusinessLogic;
using Associa.Service.BAL.Mappers;

namespace StatusUpdateEventAPI
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
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddMvcCore(options =>
            {
                options.Filters.Add(typeof(ValidateModelAttribute));
            });
            services.AddControllers()
                .AddNewtonsoftJson(setUpAction =>
                {
                    setUpAction.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                });
            services.AddAutoMapper(typeof(InvoiceUpdateEventMapper));
            services.AddControllers();
            services.AddApiVersioning();
            services.AddCors(options => options.AddPolicy("AsociaCORSPolicy", builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
            }));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Project Associa",
                    Description = "Associa API Endpoint Specifications"
                });
            });
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser()
                    .Build());
            });
            var connectionString = Configuration.GetConnectionString("AssociaContextDBString");
            services.AddEntityFrameworkNpgsql().AddDbContext<AssociaSqlContext>(opt =>
                opt.UseNpgsql(connectionString));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddTransient<IInvoiceUpdateEventRepository, InvoiceUpdateEventRepository>();
            services.AddTransient<IInvoiceUpdateEventLogic, InvoiceUpdateEventLogic>();
           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AssociaSqlContext context)
        {
            app.UseCors("AsociaCORSPolicy");
            app.UsePathBase(new PathString("/associa"));
            app.Use((context, next) =>
            {
                context.Request.PathBase = new PathString("/associa");
                return next();
            }).UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/associa/swagger/v1/swagger.json", "MyAPI");
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           context.Database.EnsureCreated();
        }
    }
}
