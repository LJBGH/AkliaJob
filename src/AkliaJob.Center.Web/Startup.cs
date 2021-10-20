using AkliaJob.Center.Web.StartupModule;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AkliaJob.Shared;

namespace AkliaJob.Center.Web
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AkliaJob.Center.Web", Version = "v1" });
            });

            //公共拓展模块注入
            services.AddCommonModule();
        }

        //Autofac模块注入
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //服务层和仓储层注入
            builder.ServicesAndRepositoryInject();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AkliaJob.Center.Web v1"));
            }

            //自定义中间件拓展
            app.CustomerMiddleware();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
