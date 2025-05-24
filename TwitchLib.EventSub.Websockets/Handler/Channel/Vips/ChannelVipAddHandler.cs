using System;
using System.Text.Json;
using TwitchLib.EventSub.Core.SubscriptionTypes.Channel;
using TwitchLib.EventSub.Websockets.Core.EventArgs;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
using TwitchLib.EventSub.Websockets.Core.Handler;
using TwitchLib.EventSub.Websockets.Core.Models;

namespace TwitchLib.EventSub.Websockets.Handler.Channel.Vips
{
    /// <summary>
    /// Handler for 'channel.vip.add' notifications
    /// </summary>
    public class ChannelVipAddHandler : INotificationHandler
    {
        /// <inheritdoc />
        public string SubscriptionType => "channel.vip.add";

        /// <inheritdoc />
        public void Handle(EventSubWebsocketClient client, string jsonString, WebsocketsEventSubMetadata metadata, JsonSerializerOptions serializerOptions)
        {
            try
            {
                var data = JsonSerializer.Deserialize<EventSubNotificationPayload<ChannelVip>>(jsonString.AsSpan(), serializerOptions);

                if (data is null)
                    throw new InvalidOperationException("Parsed JSON cannot be null!");

                client.RaiseEvent("ChannelVipAdd", new ChannelVipArgs { Payload = data, Metadata = metadata });
            }
            catch (Exception ex)
            {
                client.RaiseEvent("ErrorOccurred", new ErrorOccuredArgs { Exception = ex, Message = $"Error encountered while trying to handle {SubscriptionType} notification! Raw Json: {jsonString}" });
            }
        }
    }
}