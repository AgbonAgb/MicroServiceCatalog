using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.CataLog.Service.Entities;
//using Play.CataLog.Service.Repository;
//using Play.CataLog.Service.Settings;
//using Play.CataLog.Service.Repositories;
using Play.Common.MongoDB;
using Play.Common.Settings;

namespace Play.CataLog.Service
{
    public class Startup
    {
        private ServiceSettings _serviceSettings;//Self declared
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //The below two lines will make the Json files readable in string format when saving to Mongodb
            ////   BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
            ////  BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));
            //get values of serviceName and others
            // _serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

            //get values for MongoDb Client
            ////   services.AddSingleton(serviceprovider =>
            ////    {
            //builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            ////        var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();//Class and Appsettings
            // var mongoDbSettings = services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
            //var monGoClient = new MongoClient("mongodb://localhost:27017");
            ////       var monGoClient = new MongoClient(mongoDbSettings.conectionstrings);
            ////       return monGoClient.GetDatabase(_serviceSettings.ServiceName);//Servicename=catalog
            // "ServiceName":"CataLog"

            //// });
            //Register the repo dependency
            //services.AddSingleton<IRepository<Item>, MongoRepository>();
            //// services.AddSingleton<IRepository<Item>>(serviceprovider =>
            ////  {
            ////      var database = serviceprovider.GetService<IMongoDatabase>();
            ////       return new MongoRepository<Item>(database, "items");//pass items into the constructors
            ////   });
            //  _serviceSettings = Configuration.GetSection("ServiceName:CataLog");

            services.AddMongo()
            .AddMongoRepository<Item>("items");

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;//this allow method with Async names to run without issues
            }
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Play.CataLog.Service", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Play.CataLog.Service v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
