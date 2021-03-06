﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SmartClock.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("initlizing ClockManager");
            ClockManager.Init();
            Console.WriteLine("ClockManager Ready");
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://*:8801")
                .Build();
    }
}
