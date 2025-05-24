using System;
using System.Collections.Generic;

namespace TwitchLib.EventSub.Websockets.Core.Models
{
    // move to .Core
    // rename to EventSubPayload?

    public class EventSubNotificationPayload<T>
    {
        /// <summary>
        /// Data about the subscription the notifications belong to
        /// </summary>
        public EventSubSubscription Subscription { get; set; } = new();

        /// <summary>
        /// The event’s data.
        /// </summary>
        public T Event { get; set; }
    }
}