using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MerchandaiseDomain.AggregationModels.Contracts;
using MerchandaiseDomain.AggregationModels.EmployeeAgregate;
using MerchandaiseDomain.AggregationModels.MerchAgregate;
using MerchandaiseDomain.AggregationModels.OrdersAgregate;
using MerchandaiseDomainServices;
using MerchandaiseDomainServices.Interfaces;
using MerchandaiseGrpc.StockApi;
using MerchandaiseGrpcClient;
using MerchandaiseInfrastructure;
using MerchandiseService.GrpcServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MerchandiseService
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
            services.AddSingleton<IMerchService, MerchService>();
            services.AddSingleton<IStockClient, StockClient>();
            services.AddSingleton<IStockGateway, StockGateway>();
            services.AddSingleton<IOrdersRepository, OrdersRepository>();
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IMerchRepository, MerchRepository>();
            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();

            services.AddGrpcClient<StockApiGrpc.StockApiGrpcClient>(o =>
            {
                o.Address = new Uri(Configuration.GetSection("StockApiUrl").Value);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<MerchandiseGrpcService>();
                endpoints.MapControllers();
            });
        }
    }
}