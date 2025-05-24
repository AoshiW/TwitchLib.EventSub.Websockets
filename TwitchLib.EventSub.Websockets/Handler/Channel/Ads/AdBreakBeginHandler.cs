using System;
using System.Text.Json;
using TwitchLib.EventSub.Core.SubscriptionTypes.Channel;
using TwitchLib.EventSub.Websockets.Core.EventArgs;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
using TwitchLib.EventSub.Websockets.Core.Handler;
using TwitchLib.EventSub.Websockets.Core.Models;

namespace TwitchLib.EventSub.Websockets.Handler.Channel.Ads
{
    /// <summary>
    /// Handler for 'channel.ad_break.begin' notifications
    /// </summary>
    public class AdBreakBeginHandler : INotificationHandler
    {
        /// <inheritdoc />
        public string SubscriptionType => "channel.ad_break.begin";

        /// <inheritdoc />
        public void Handle(EventSubWebsocketClient client, string jsonString, WebsocketsEventSubMetadata metadata, JsonSerializerOptions serializerOptions)
        {
            try
            {
                var data = JsonSerializer.Deserialize<EventSubNotificationPayload<ChannelAdBreakBegin>>(jsonString.AsSpan(), serializerOptions);
                if (data is null)
                    throw new InvalidOperationException("Parsed JSON cannot be null!");
                client.RaiseEvent("ChannelAdBreakBegin", new ChannelAdBreakBeginArgs { Payload = data, Metadata = metadata });
            }
            catch (Exception ex)
            {
                client.RaiseEvent("ErrorOccurred", new ErrorOccuredArgs { Exception = ex, Message = $"Error encountered while trying to handle {SubscriptionType} notification! Raw Json: {jsonString}" });
            }
        }
    }
}
