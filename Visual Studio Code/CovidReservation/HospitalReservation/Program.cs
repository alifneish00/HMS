using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace HospitalReservation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(opt => {
                        opt.ListenLocalhost(44321);
                        opt.ListenLocalhost(44322, conf => conf.UseHttps());

                        var addresses = Dns.GetHostEntry((Dns.GetHostName()))
                                        .AddressList
                                        .Where(x => x.AddressFamily == AddressFamily.InterNetwork)
                                        .Select(x => x.ToString())
                                        .ToArray();

                        foreach(var address in addresses)
                        {
                            opt.Listen(IPAddress.Parse(address), 44321);
                            opt.Listen(IPAddress.Parse(address), 44322, conf => conf.UseHttps());
                        }   

                        
                    });
                });
    }
}
