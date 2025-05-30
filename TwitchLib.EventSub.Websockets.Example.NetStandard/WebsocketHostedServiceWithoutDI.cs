using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwitchLib.Api;
using TwitchLib.EventSub.Websockets.Core.EventArgs;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;

namespace TwitchLib.EventSub.Websockets.Example.NetStandard
{
    public class WebsocketHostedServiceWithoutDI : IHostedService
    {
        private readonly ILogger<WebsocketHostedService> _logger;
        private readonly EventSubWebsocketClient _eventSubWebsocketClient;
        private readonly TwitchAPI _twitchApi = new();
        private string _userId;
        
        public WebsocketHostedServiceWithoutDI(ILogger<WebsocketHostedService> logger, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            _eventSubWebsocketClient = new EventSubWebsocketClient(loggerFactory);
            
            _eventSubWebsocketClient.WebsocketConnected += OnWebsocketConnected;
            _eventSubWebsocketClient.WebsocketDisconnected += OnWebsocketDisconnected;
            _eventSubWebsocketClient.WebsocketReconnected += OnWebsocketReconnected;
            _eventSubWebsocketClient.ErrorOccurred += OnErrorOccurred;

            _eventSubWebsocketClient.ChannelFollow += OnChannelFollow;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _eventSubWebsocketClient.ConnectAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _eventSubWebsocketClient.DisconnectAsync();
        }
        
        private async Task OnErrorOccurred(object sender, ErrorOccuredArgs e)
        {
            _logger.LogError($"Websocket {_eventSubWebsocketClient.SessionId} - Error occurred!");
        }

        private async Task OnChannelFollow(object sender, ChannelFollowArgs e)
        {
            var eventData = e.Notification.Payload.Event;
            _logger.LogInformation($"{eventData.UserName} followed {eventData.BroadcasterUserName} at {eventData.FollowedAt}");
        }
        
        private async Task OnWebsocketConnected(object sender, WebsocketConnectedArgs e)
        {
            _logger.LogInformation($"Websocket {_eventSubWebsocketClient.SessionId} connected!");

            if (!e.IsRequestedReconnect)
            {
                // subscribe to topics
                // Get ClientId and ClientSecret by register an Application here: https://dev.twitch.tv/console/apps
                // https://dev.twitch.tv/docs/authentication/register-app/
                _twitchApi.Settings.ClientId = "YOUR_APP_CLIENT_ID";
                // Get Application Token with Client credentials grant flow.
                // https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#client-credentials-grant-flow
                _twitchApi.Settings.AccessToken = "YOUR_APPLICATION_ACCESS_TOKEN";

                // You need the UserID for the User/Channel you want to get Events from.
                // You can use await _api.Helix.Users.GetUsersAsync() for that.
                _userId = "USER_ID";
            }
        }

        private async Task OnWebsocketDisconnected(object sender, EventArgs e)
        {
            _logger.LogError($"Websocket {_eventSubWebsocketClient.SessionId} disconnected!");

            // Don't do this in production. You should implement a better reconnect strategy
            while (!await _eventSubWebsocketClient.ReconnectAsync())
            {
                _logger.LogError("Websocket reconnect failed!");
                await Task.Delay(1000);
            }
        }

        private async Task OnWebsocketReconnected(object sender, EventArgs e)
        {
            _logger.LogWarning($"Websocket {_eventSubWebsocketClient.SessionId} reconnected");
        }
    }
}