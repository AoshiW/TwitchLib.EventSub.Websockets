using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TwitchLib.EventSub.Core.SubscriptionTypes.Channel;
using TwitchLib.EventSub.Websockets.Core.EventArgs;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
using TwitchLib.EventSub.Websockets.Core.Handler;
using TwitchLib.EventSub.Websockets.Core.Models;

namespace TwitchLib.EventSub.Websockets.Handler.Channel.SuspiciousUser
{
    /// <summary>
    /// Handler for 'channel.suspicious_user.message' notifications
    /// </summary>
    public class ChannelSuspiciousUserMessageHandler : INotificationHandler
    {
        /// <inheritdoc />
        public string SubscriptionType => "channel.suspicious_user.message";

        /// <inheritdoc />
        public void Handle(EventSubWebsocketClient client, string jsonString, WebsocketsEventSubMetadata metadata, JsonSerializerOptions serializerOptions)
        {
            try
            {
                var data = JsonSerializer.Deserialize<EventSubNotificationPayload<ChannelSuspiciousUserMessage>>(jsonString.AsSpan(), serializerOptions);

                if (data is null)
                    throw new InvalidOperationException("Parsed JSON cannot be null!");

                client.RaiseEvent("ChannelSuspiciousUserMessage", new ChannelSuspiciousUserMessageArgs { Payload = data, Metadata = metadata });
            }
            catch (Exception ex)
            {
                client.RaiseEvent("ErrorOccurred", new ErrorOccuredArgs { Exception = ex, Message = $"Error encountered while trying to handle {SubscriptionType} notification! Raw Json: {jsonString}" });
            }
        }
    }
}
