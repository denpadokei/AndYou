using ChatCore.Models.Twitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndYou.Extentions
{
    public static class TwitchMessageExtention
    {
        public static TwitchMessageType GetMessageType(this TwitchMessage message)
        {
#if DEBUG
            foreach (var item in message.Metadata) {
                Plugin.Log.Debug($"{item.Key} : {item.Value}");
            }
#endif
            if (message.Metadata.TryGetValue("msg-id", out var id)) {
                if (id == "raid") {
                    return TwitchMessageType.Raid;
                }
                else if (id == "sub" || id == "resub") {
                    return TwitchMessageType.Subscribe;
                }
                else if (id == "subgift") {
                    return TwitchMessageType.Gift;
                }
            }
            if (0 < message.Bits) {
                return TwitchMessageType.Bits;
            }
            return TwitchMessageType.Other;
        }
    }
}

namespace AndYou
{
    public enum TwitchMessageType
    {
        Other,
        Bits,
        Subscribe,
        Raid,
        Gift
    }
}