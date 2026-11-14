# AndYou

[![Build and Release](https://github.com/denpadokei/AndYou/actions/workflows/build-and-release.yml/badge.svg)](https://github.com/denpadokei/AndYou/actions/workflows/build-and-release.yml)

Twitch 配信中に Bits・サブスク・ギフト・レイドで応援してくれた視聴者名を記録し、プレイ中にステージ上を流れるテキストとして表示する MOD です。

## 機能

- ChatCore 経由で Twitch のチャットメッセージを監視し、`msg-id` (sub / resub / subgift / raid) と Bits から応援の種類を判定
- 種類ごとに `UserData/AndYou` の `Subscribe.txt` / `Gifts.txt` / `Bits.txt` / `Raid.txt` へ名前を追記 (ゲームを終了しても保持)
- `UserData/AndYou/AndYou.txt` のテンプレートを読み、`%SUBSCRIBER%` `%GIFT%` `%BITS_SENDER%` `%RAIDER%` を名前一覧に置換
- プレイ中、曲の進行に合わせてテキストを手前から奥へ移動させながらフローティングスクリーンに表示
- レベル選択画面の Mod タブに「And You」タブを追加し、Enable の切り替えと記録の全消去ができる

## 動作環境

| 項目 | 内容 |
| --- | --- |
| Beat Saber | 1.16.2 |
| 依存 MOD | BSIPA 4.0.5 / SiraUtil 2.5.5 / ChatCore 2.1.2 |

## インストール

1. Beat Saber を 1.16.2 に合わせる (BSManager などのバージョン管理ツールを推奨)
2. BSIPA と上記の依存 MOD をインストールする
3. [Releases](https://github.com/denpadokei/AndYou/releases) から `AndYou-0.0.2-bs1.16.2.zip` をダウンロードし、`Plugins` フォルダをゲームフォルダへ上書きコピーする

## 使い方

1. ChatCore で Twitch アカウントを設定する (EnhancedStreamChat や SongRequestManager など、ChatCore を使う MOD がすでに動いている場合は追加の設定は不要です)
2. レベル選択画面の Mod タブ →「And You」で **Enable** を ON にする
3. プレイを開始すると `UserData/AndYou/AndYou.txt` の内容がステージ上に表示される

`AndYou.txt` は初回起動時に自動で作成され、次の置換文字が使えます。

| 置換文字 | 置換される内容 |
| --- | --- |
| `%SUBSCRIBER%` | サブスク (サブギフトを含む) してくれた人 |
| `%GIFT%` | ギフトを送ってくれた人 |
| `%BITS_SENDER%` | Bits を送ってくれた人 |
| `%RAIDER%` | レイドしてくれた人 |

記録された名前は上記の txt に 1 行 1 名で保存されます。「Names all Clear」を押すと記録とテキストがすべて消去されます。

## 設定

| 項目 | 型 | 既定値 | 説明 |
| --- | --- | --- | --- |
| Enable | bool | false | プレイ中にテキストを表示するかどうか |
| ScreenYPos | float | 0.2 | フローティングスクリーンの高さ (m) |

設定は `UserData/AndYou.json` に保存されます。`ScreenYPos` はゲーム内 UI にはないため、このファイルを直接編集して変更します。

## ビルド

```powershell
git clone https://github.com/denpadokei/AndYou.git
cd AndYou
dotnet build .\AndYou\AndYou.csproj -c Release
```

`BeatSaberDir` (または `GameDirectory`) に Beat Saber のインストール先を設定すると、ゲームの参照アセンブリを解決できます。既定では `..\Refs` フォルダが参照先になります。

GitHub Actions の `build-and-release.yml` が push ごとにビルドし、`X.X.X` 形式のタグを push するとリリースを作成します。

## ライセンス

MIT
