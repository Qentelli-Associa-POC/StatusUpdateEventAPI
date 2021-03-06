﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
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
            string[] origins = new string[] { "localhost", "ae559145148674de98b30fbd20eb2e3a-b2aad68415df95b6.elb.us-east-2.amazonaws.com" };
            services.AddCors(options => options.AddPolicy("AsociaUpdateStatusCORSPolicy", builder =>
            {
                builder.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
            }));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Project Associa",
                    Description = "Associa Update Status API Endpoint Specifications"
                });
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
            app.UseCors("AsociaUpdateStatusCORSPolicy");
            app.UsePathBase(new PathString("/status-update"));
            app.Use((httpContext, next) =>
            {
                httpContext.Request.PathBase = new PathString("/status-update");

                return next();
            });
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", "Associa Services");
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
