namespace TwitchLib.EventSub.Websockets.Core.Models
{
    // move to .Core

    public class EventSubTransport
    {
        public string Method { get; set; }
        public string? Callback { get; set; }

        public string WebsocketId { get; set; }
        public string? ConduitId { get; set; }
    }
}