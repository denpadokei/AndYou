using AndYou.Extentions;
using ChatCore.Interfaces;
using ChatCore.Models.Twitch;
using ChatCore.Services;
using ChatCore.Services.Twitch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace AndYou.Models
{
    public class ChatManager : IInitializable, IDisposable
    {
        public static readonly string ConfigRootPath = Path.Combine(Environment.CurrentDirectory, "UserData", "AndYou");
        public static readonly string SubScribePath = Path.Combine(ConfigRootPath, "Subscribe.txt");
        public static readonly string BitsPath = Path.Combine(ConfigRootPath, "Bits.txt");
        public static readonly string GiftsPath = Path.Combine(ConfigRootPath, "Gifts.txt");
        public static readonly string RaidPath = Path.Combine(ConfigRootPath, "Raid.txt");


        public HashSet<string> SubScribers { get; } = new HashSet<string>();
        public HashSet<string> GiftSenders { get; } = new HashSet<string>();
        public HashSet<string> BitsSenders { get; } = new HashSet<string>();
        public HashSet<string> Raiders { get; } = new HashSet<string>();

        public void Initialize()
        {
            this.LoadNames();
            var instance = ChatCore.ChatCoreInstance.Create();
            this.chatService = instance.RunAllServices();
            this.chatService.OnTextMessageReceived += this.ChatService_OnTextMessageReceived;
        }

        public void LoadNames()
        {
            try {
                if (!Directory.Exists(ConfigRootPath)) {
                    Directory.CreateDirectory(ConfigRootPath);
                }

                if (!File.Exists(SubScribePath)) {
                    using (var fs = File.Create(SubScribePath)) { };
                }
                foreach (var name in File.ReadAllLines(SubScribePath)) {
                    this.SubScribers.Add(name);
                }

                if (!File.Exists(BitsPath)) {
                    using (var fs = File.Create(BitsPath)) { };
                }
                foreach (var name in File.ReadAllLines(BitsPath)) {
                    this.BitsSenders.Add(name);
                }

                if (!File.Exists(GiftsPath)) {
                    using (var fs = File.Create(GiftsPath)) { };
                }
                foreach (var name in File.ReadAllLines(GiftsPath)) {
                    this.GiftSenders.Add(name);
                }

                if (!File.Exists(RaidPath)) {
                    using (var fs = File.Create(RaidPath)) { };
                }
                foreach (var name in File.ReadAllLines(RaidPath)) {
                    this.Raiders.Add(name);
                }
            }
            catch (Exception e) {
                Plugin.Log.Error(e);
            }
        }

        public void WriteNames()
        {
            if (!Directory.Exists(ConfigRootPath)) {
                Directory.CreateDirectory(ConfigRootPath);
            }

            File.WriteAllLines(SubScribePath, this.SubScribers.OrderBy(x => x));
            File.WriteAllLines(GiftsPath, this.GiftSenders.OrderBy(x => x));
            File.WriteAllLines(BitsPath, this.BitsSenders.OrderBy(x => x));
            File.WriteAllLines(RaidPath, this.Raiders.OrderBy(x => x));
        }

        private void ChatService_OnTextMessageReceived(ChatCore.Interfaces.IChatService arg1, ChatCore.Interfaces.IChatMessage arg2)
        {
            if (arg2 is TwitchMessage twitch) {
                switch (twitch.GetMessageType()) {
                    case TwitchMessageType.Bits:
                        this.BitsSenders.Add(twitch.Sender.DisplayName);
                        break;
                    case TwitchMessageType.Subscribe:
                        this.SubScribers.Add(twitch.Sender.DisplayName);
                        break;
                    case TwitchMessageType.Raid:
                        this.Raiders.Add(twitch.Sender.DisplayName);
                        break;
                    case TwitchMessageType.Gift:
                        this.GiftSenders.Add(twitch.Sender.DisplayName);
                        break;
                    default:
                        break;
                }
                _ = Task.Run(() => this.WriteNames());
            }
        }

        public void AllClear()
        {
            this.GiftSenders.Clear();
            this.Raiders.Clear();
            this.SubScribers.Clear();
            this.BitsSenders.Clear();
            this.WriteNames();
        }

        private bool disposedValue;
        private ChatServiceMultiplexer chatService;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    this.chatService.OnTextMessageReceived -= this.ChatService_OnTextMessageReceived;
                    this.WriteNames();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~ChatManager()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
