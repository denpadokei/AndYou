using AndYou.Configuration;
using AndYou.Models;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AndYou.Views
{
    [HotReload]
    public class AndYouScreenController : BSMLAutomaticViewController, IInitializable
    {
        private bool SetProperty<T>(ref T oldvalue, T newValue, [CallerMemberName] string member = null)
        {
            if (EqualityComparer<T>.Default.Equals(oldvalue, newValue)) {
                return false;
            }
            oldvalue = newValue;
            this.OnPropertyChanged(new PropertyChangedEventArgs(member));
            return true;
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e) => this.NotifyPropertyChanged(e.PropertyName);

        /// <summary>説明 を取得、設定</summary>
        private string andyouText_;
        [UIValue("and-you-text")]
        /// <summary>説明 を取得、設定</summary>
        public string AndYouText
        {
            get => this.andyouText_;

            set => this.SetProperty(ref this.andyouText_, value);
        }

        #region member
        private AudioTimeSyncController audioTimeSyncController;
        private ChatManager chatManager;
        private FloatingScreen _floatingScreen;
        [UIComponent("text")]
        private readonly TextMeshProUGUI _staffText;
        [UIComponent("vertical-group")]
        private readonly VerticalLayoutGroup verticalLayoutGroup;
        private static readonly string _textFile = Path.Combine(Environment.CurrentDirectory, "UserData", "AndYou", "AndYou.txt");
        private float startPosZ;
        private float endPosZ;
        private float moveLength;
        private float screenYPos;
        #endregion
        // These methods are automatically called by Unity, you should remove any you aren't using.
        [Inject]
        private void Constractor(AudioTimeSyncController audio, ChatManager manager)
        {
            this.audioTimeSyncController = audio;
            this.chatManager = manager;
        }
        #region UnityMethods
        private IEnumerator Start()
        {
            yield return new WaitWhile(() => !this.verticalLayoutGroup || !this._staffText);
            FontManager.TryGetTMPFontByFamily("Segoe UI", out var font);
            this._staffText.font = font;
            if (!Directory.Exists(Path.GetDirectoryName(_textFile))) {
                Directory.CreateDirectory(Path.GetDirectoryName(_textFile));
                using (var _ = File.Create(_textFile)) {
                }
            }
            var text = File.ReadAllText(_textFile);
            this.SetText(this.BuildMessage(text));
            Plugin.Log.Debug($"{this._staffText.preferredHeight}");
            this._floatingScreen.ScreenSize = new Vector2(50f, this._staffText.preferredHeight + 2);
            this.startPosZ = -0.3f - (this._floatingScreen.ScreenSize.y / 2f);
            this.endPosZ = 8f + (this._floatingScreen.ScreenSize.y / 2f);
            this.moveLength = this.endPosZ - this.startPosZ;
            this.screenYPos = PluginConfig.Instance.ScreenYPos;
            this._floatingScreen.transform.position = new Vector3(0f, this.screenYPos, this.startPosZ);
        }

        private string BuildMessage(string fileText)
        {
            var builder = new StringBuilder(fileText);
            var donates = new StringBuilder();
            foreach (var subs in this.chatManager.SubScribers.OrderBy(x => x)) {
                donates.Append(subs);
                donates.Append("\r\n");
            }
            builder.Replace("%SUBSCRIBER%", donates.ToString());
            donates.Clear();

            foreach (var subs in this.chatManager.GiftSenders.OrderBy(x => x)) {
                donates.Append(subs);
                donates.Append("\r\n");
            }
            builder.Replace("%GIFT%", donates.ToString());
            donates.Clear();

            foreach (var subs in this.chatManager.BitsSenders.OrderBy(x => x)) {
                donates.Append(subs);
                donates.Append("\r\n");
            }
            builder.Replace("%BITS_SENDER%", donates.ToString());
            donates.Clear();


            foreach (var subs in this.chatManager.Raiders.OrderBy(x => x)) {
                donates.Append(subs);
                donates.Append("\r\n");
            }
            builder.Replace("%RAIDER%", donates.ToString());
            donates.Clear();

            return builder.ToString();
        }

        private void Update() => this.MoveScreen();
        #endregion
        private void MoveScreen()
        {
            if (this.startPosZ == 0 || this.endPosZ == 0) {
                return;
            }
            var progress = this.audioTimeSyncController.songTime / this.audioTimeSyncController.songLength;
            var diff = this.moveLength * progress;
            this._floatingScreen.ScreenPosition = new Vector3(0f, screenYPos, this.startPosZ + diff);
        }
        public void SetText(string value)
        {
            this.AndYouText = value;
            Plugin.Log.Debug(this.AndYouText);
        }
        

        public void Initialize()
        {
            if (PluginConfig.Instance.Enable != true) {
                return;
            }
            this._floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(50f, 100f), false, new Vector3(0f, 0.2f, -50f), Quaternion.Euler(90f, 0f, 0f));
            this._floatingScreen.transform.localScale = Vector3.one;
            this._floatingScreen.SetRootViewController(this, AnimationType.None);
            this._floatingScreen.gameObject.layer = 5;
            this._floatingScreen.gameObject.GetComponent<Canvas>().sortingLayerID = 0;
        }
    }
}
