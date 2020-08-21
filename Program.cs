using BlinkerAPI.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;

namespace BlinkerAPI
{
    public class Program
    {
        public static readonly string FAQ =
            $"How to:{Environment.NewLine}" +
            $"\tFirst argument sets Pin number for LED (6 by default){Environment.NewLine}" +
            $"\tSecond argument sets light time for LED in milliseconds (200 by default){Environment.NewLine}" +
            $"\tThird argument sets blackout time for LED in milliseconds (10 by default){Environment.NewLine}";

        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (HelpRequired(args[0]))
                {
                    Console.WriteLine(FAQ);
                    return;
                }

                if (int.TryParse(args[0], out var ledPin))
                {
                    GpioConfiguration.LedPin = ledPin;
                }

                if (args.Length > 1 && int.TryParse(args[1], out var lightTime))
                {
                    GpioConfiguration.LightTimeInMilliseconds = lightTime;
                }

                if (args.Length > 2 && int.TryParse(args[2], out var dimTime))
                {
                    GpioConfiguration.DimTimeInMilliseconds = dimTime;
                }
            }

            Console.WriteLine($"Led = {GpioConfiguration.LedPin}, " +
                $"Light = {GpioConfiguration.LightTimeInMilliseconds}, " +
                $"Dim = {GpioConfiguration.DimTimeInMilliseconds}");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Loopback, 5001);
                    })
                    .UseStartup<Startup>();
                });

        private static bool HelpRequired(string param)
        {
            return param == "-h" || param == "--help" || param == "/?";
        }
    }
}
