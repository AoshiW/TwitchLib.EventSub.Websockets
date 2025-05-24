using System;
using System.Text.Json;
using TwitchLib.EventSub.Websockets.Core.EventArgs;
using TwitchLib.EventSub.Websockets.Core.Handler;
using TwitchLib.EventSub.Websockets.Core.Models;

namespace TwitchLib.EventSub.Websockets.Handler
{
    /// <summary>
    /// Handler for 'revocation' notifications
    /// </summary>
    public class RevocationHandler : INotificationHandler
    {
        /// <inheritdoc />
        public string SubscriptionType => "revocation";

        /// <inheritdoc />
        public void Handle(EventSubWebsocketClient client, string jsonString, WebsocketsEventSubMetadata metadata, JsonSerializerOptions serializerOptions)
        {
            var data = JsonSerializer.Deserialize<EventSubNotificationPayload<object>>(jsonString.AsSpan(), serializerOptions);

            if (data is null)
                throw new InvalidOperationException("Parsed JSON cannot be null!");

            client.RaiseEvent("Revocation", new RevocationArgs { Payload = data, Metadata = metadata });
        }
    }
}