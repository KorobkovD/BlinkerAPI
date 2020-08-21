using BlinkerAPI.Controllers;
using BlinkerAPI.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Device.Gpio;
using System.Threading;

namespace BlinkerAPI
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var applicationLifetime = app.ApplicationServices.GetRequiredService<Microsoft.Extensions.Hosting.IApplicationLifetime>();
            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void OnShutdown()
        {
            var gpioController = new GpioController();
            var led = GpioConfiguration.LedPin;
            if (gpioController.IsPinOpen(led))
            {
                Thread.Sleep(GpioConfiguration.LightTimeInMilliseconds);
                LedController.StopBlinking();
                gpioController.ClosePin(led);
                Console.WriteLine($"{Environment.NewLine}Pin successfully closed");
            }
        }
    }
}
