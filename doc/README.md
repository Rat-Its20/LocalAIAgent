# LocalAIAgent

### プロジェクト概要
Foundary Localを利用した構築の方法の検討・性能検証用のデモアプリケーション

### アプリケーションの構成
![Architecture](image/Architecture.drawio.png)

| プロジェクト名 | 種別 | 概要 |
| ---- | ---- | ---- |
| LocalAgentApp | WPF | UI層としてViewとViewModelを提供 |
| LocalAgentApp.Model | C# Library | その他の機能を提供 |
| LocalAgentApp.AI | C# Library | AI関連の機能を提供 |

## OSS

### - LocalAgentApp

| OSS | ライセンス | バージョン | 概要 |
| ---- | ---- | ---- | ---- |
| Autofac | MIT | 9.1.0 | DIコンテナ本体 |
| Autofac.Extensions.DependencyInjection | MIT | 11.0.0 | DIコンテナ・アダプター |
| Autofac.Extras.DynamicProxy | MIT | 7.1.0 | ログ・計測用 |
| Microsoft.Xaml.Behaviors.Wpf | MIT | 1.1.142 | XAML拡張OSS |
| ReactiveProperty | MIT | 9.8.0 | ReactiveProperty |
| ReactiveProperty.Core | MIT | 9.8.0 | ReactiveProperty |
| ReactiveProperty.WPF | MIT | 9.8.0 | ReactiveProperty |

### - LocalAgentApp.Model

| OSS | ライセンス | バージョン | 概要 |
| ---- | ---- | ---- | ---- |
| Microsoft.AI.Foundry.Local | MIT | 1.2.1 | Foundary Local 本体 |

### - LocalAgentApp

| OSS | ライセンス | バージョン | 概要 |
| ---- | ---- | ---- | ---- |
| System.Text.Json | MIT | 10.0.8 | JSONを扱うためのライブラリ |

## AIモデル
下記のモデルはどのモデルであっても同梱可能、商用利用も問題なし。
| OSS | ライセンス | クレジット表記 |
| ---- | ---- | ---- |
| Phi-4-mini-instruct | MIT | LICENSE |
| Qwen-7B | Apache License 2.0 | LICENSE ＆ NOTICE |
| Mistral-7B-v0.1t | Apache License 2.0 | LICENSE ＆ NOTICE |