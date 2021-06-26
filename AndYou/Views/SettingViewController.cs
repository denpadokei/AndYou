using AndYou.Configuration;
using AndYou.Models;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace AndYou.Views
{
    internal class SettingViewController : BSMLResourceViewController, IInitializable, IDisposable
    {
        private bool disposedValue;
        private ChatManager manager;
        [UIValue("enable")]
        public bool Enable
        {
            get => PluginConfig.Instance.Enable;
            set
            {
                if (PluginConfig.Instance.Enable == value) {
                    return;
                }
                PluginConfig.Instance.Enable = value;
                this.NotifyPropertyChanged();
            }
        }
        // For this method of setting the ResourceName, this class must be the first class in the file.
        public override string ResourceName => string.Join(".", GetType().Namespace, GetType().Name);

        [UIAction("all-clear")]
        public void AllClearClick()
        {
            this.manager.AllClear();
        }

        [Inject]
        public void Constractor(ChatManager chatManager)
        {
            this.manager = chatManager;
        }

        public void Initialize()
        {
            GameplaySetup.instance.AddTab("And You", this.ResourceName, this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    GameplaySetup.instance.RemoveTab("And You");
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~SettingViewController()
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
