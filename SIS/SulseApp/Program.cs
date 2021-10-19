using SIS.HTTP;
using SIS.MvcFramework;
using SulseApp.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SulseApp
{
    public static class Program
    {
        public static async Task Main()
        {

            await WebHost.StartAsync(new StartUp());
        }
    }
}
