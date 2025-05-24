using TwitchLib.EventSub.Websockets.Core.Models;

namespace TwitchLib.EventSub.Websockets.Core.EventArgs
{
    // move to .Core

    public abstract class TwitchLibEventSubEventArgs<T> : System.EventArgs 
    {
        public EventSubMetadata Metadata { get; set; } = new();
        public EventSubNotificationPayload<T> Payload { get; set; } = new();
    }
}