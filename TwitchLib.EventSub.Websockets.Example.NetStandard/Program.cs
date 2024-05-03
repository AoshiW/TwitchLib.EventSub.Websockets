﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwitchLib.EventSub.Websockets.Extensions;

namespace TwitchLib.EventSub.Websockets.Example.NetStandard
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            //Add services to the container.
            builder.Services.AddLogging();
            builder.Services.AddTwitchLibEventSubWebsockets();
            builder.Services.AddHostedService<WebsocketHostedService>();

            var app = builder.Build();

            app.Run();
        }
    }
}