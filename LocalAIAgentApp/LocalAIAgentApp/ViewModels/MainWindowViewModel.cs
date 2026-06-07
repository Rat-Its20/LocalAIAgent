using Autofac;
using LocalAIAgentApp.AI;
using LocalAIAgentApp.Model.Entity.Json;
using LocalAIAgentApp.Model.Services;
using LocalAIAgentApp.Services;
using LocalAIAgentApp.ViewModels.Base;
using LocalAIAgentApp.ViewModels.Pages;
using LocalAIAgentApp.Views;
using LocalAIAgentApp.Views.Pages;
using Reactive.Bindings;
using System.Diagnostics;

namespace LocalAIAgentApp.ViewModels
{
    public class MainWindowViewModel : WindowViewModelBase
    {
        /// <summary>
        /// Field：ILifetimeScope は、依存性注入のスコープを管理するためのプロパティです。
        /// </summary>
        private ILifetimeScope _lifetimeScope { get; set; }

        /// <summary>
        /// Field：AppSettings は、アプリケーションの設定を格納するためのプロパティです。
        /// </summary>
        private AppSettings _appSettings { get; set; }

        #region << Service >>

        /// <summary>
        /// Service：DialogService
        /// </summary>
        private DialogService _dialogService { get; set; }

        /// <summary>
        /// Service：FileService
        /// </summary>
        private FileService _fileService { get; set; }

        /// <summary>
        /// Service：FoundryLocalFacade
        /// </summary>
        private FoundryLocalFacade _foundryLocalFacade { get; set; }

        #endregion << Service >>

        #region << Property >>

        /// <summary>
        /// test
        /// </summary>
        public ReactiveProperty<string> Test { get; set; } = new ReactiveProperty<string>("TEST");

        /// <summary>
        /// 
        /// </summary>
        public ReactiveProperty<bool> IsEnableTestButton { get; set; } = new ReactiveProperty<bool>(false);

        #endregion << Property >>

        #region << Command >>

        /// <summary>
        /// TestCommand は、テスト用のコマンドです。
        /// </summary>
        public ReactiveCommand TestCommand { get; set; } = new ReactiveCommand();

        /// <summary>
        /// LoadCommand は、ウィンドウのロードイベントに対応するコマンドです。
        /// </summary>
        public ReactiveCommand LoadCommand { get; set; } = new ReactiveCommand();

        /// <summary>
        /// ClosingCommand は、ウィンドウのクローズイベントに対応するコマンドです。
        /// </summary>
        public ReactiveCommand ClosingCommand { get; set; } = new ReactiveCommand();

        /// <summary>
        /// CloseCommand は、ウィンドウを閉じるためのコマンドです。
        /// </summary>
        public ReactiveCommand CloseCommand { get; set; } = new ReactiveCommand();

        /// <summary>
        /// MinimizeCommand は、ウィンドウを最小化するためのコマンドです。
        /// </summary>
        public ReactiveCommand MinimizeCommand { get; set; } = new ReactiveCommand();

        /// <summary>
        /// MaximizeCommand は、ウィンドウを最大化するためのコマンドです。
        /// </summary>
        public ReactiveCommand MaximizeCommand { get; set; } = new ReactiveCommand();

        #endregion << Command >>

        #region << View >>

        /// <summary>
        /// ViewModel：ChatControlViewModel
        /// </summary>
        public ReactiveProperty<ChatControlViewModel> ChatControlViewModel { get; set; } = new ReactiveProperty<ChatControlViewModel>();

        #endregion << View >>

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel(ILifetimeScope lifetimeScope, DialogService dialogService, FileService fileService, FoundryLocalFacade foundryLocalFacade) : base()
        {
            _lifetimeScope = lifetimeScope;

            
            _dialogService = dialogService;

            _foundryLocalFacade = foundryLocalFacade;

            // FileService 
            {
                _fileService = fileService;

                // 初期化処理：ファイルの存在確認と AppSettings の読み込み
                _appSettings = _fileService.InitializeFolderConfigure(AppDomain.CurrentDomain.BaseDirectory);
            }

            SetSubscribe();
        }

        /// <summary>
        /// Initialize メソッドは、ViewModel の初期化処理を行うためのメソッドです。
        /// </summary>
        public override void Initialize()
        {
            Debug.WriteLine("MainWindowViewModel: Initialize method called");
        }

        /// <summary>
        /// SetSubscribe メソッドは、ウィンドウのロードイベントに対応するコマンドの購読を設定するためのメソッドです。
        /// </summary>
        public override void SetSubscribe()
        {
            // TestCommand の購読を設定
            TestCommand.Subscribe(async() =>
            {
                // テスト用の処理を実行するためのメソッドを呼び出す
                await _foundryLocalFacade.TestChat();
            });

            // タイトルバーの左クリックイベントに対応するコマンドの購読を設定
            TitleBarMouseLeftDownCommand.Subscribe(e =>
            {
                // タイトルバーの左クリックイベントを処理するためのメソッドを呼び出す
                OnTitleBarMouseLeftButtonDown(App.Current.MainWindow, e);
            });

            // Window 読み込み時
            LoadCommand.Subscribe(() =>
            {
                // モデルの初期設定が必要かを判定
                {
                    // モデルフォルダが存在する場合の確認
                    if (!_appSettings.IsCheckModel)
                    {
                        // TODO：モデルの存在確認を行う
                    }
                }

                // Pageの初期化
                {
                    ChatControlViewModel.Value = _lifetimeScope.Resolve<ChatControlViewModel>();
                }

                // モデルの初期設定が必要な場合は、モデル設定ウィンドウを開く
                if (_appSettings.IsCheckModel)
                {
                    // モデル設定ウィンドウを開く
                    OpenModelSettingWindow();
                }
            });

            // Window クローズ前
            ClosingCommand.Subscribe(() =>
            {
                // FileService を使用して、アプリケーションの設定を保存する。
                {
                    // AppSettings
                    _fileService.SaveAppSettings(_appSettings);

                    // FoundryLocalFacade
                    _foundryLocalFacade.Dispose();
                }
            });

            // ウィンドウを閉じる処理
            CloseCommand.Subscribe(() =>
            {
                App.Current.Shutdown() ;
            });

            // ウィンドウを最小化する処理
            MinimizeCommand.Subscribe(() =>
            {
                App.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
            });

            // ウィンドウを最大化/元に戻す処理
            MaximizeCommand.Subscribe(() =>
            {
                if (App.Current.MainWindow.WindowState == System.Windows.WindowState.Maximized)
                {
                    App.Current.MainWindow.WindowState = System.Windows.WindowState.Normal;
                }
                else
                {
                    App.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;
                }
            });
        }

        #region << Window >>

        /// <summary>
        /// OpenModelSettingWindow は ModelSettingWindow を開くためのメソッドです。
        /// </summary>
        private void OpenModelSettingWindow()
        {
            // ModelSettingWindowViewModel と ModelSettingWindow を依存性注入コンテナから解決
            ModelSettingWindow modelSettingWindow = _lifetimeScope.Resolve<ModelSettingWindow>();
            ModelSettingWindowViewModel modelSettingWindowViewModel = _lifetimeScope.Resolve<ModelSettingWindowViewModel>();

            // ViewModel関連の処理
            {
                modelSettingWindowViewModel.SetServices(_appSettings, _dialogService, _fileService, _foundryLocalFacade);
            }

            // Window関連の処理
            {
                modelSettingWindow.Owner = App.Current.MainWindow;
                modelSettingWindow.DataContext = modelSettingWindowViewModel;

                // モデル設定ウィンドウが閉じられたときの処理を追加
                modelSettingWindow.Closing += async (sender, e) =>
                {
                    // モデル設定ウィンドウが閉じられたときの処理をここに追加
                    DialogWindow dialogWindow = _lifetimeScope.Resolve<DialogWindow>();
                    DialogWindowViewModel dialogWindowViewModel = _lifetimeScope.Resolve<DialogWindowViewModel>();

                    dialogWindow.Owner = App.Current.MainWindow;
                    dialogWindow.DataContext = dialogWindowViewModel;

                    try
                    {
                        // ダイアログを表示
                        dialogWindow.Show();

                        await Task.Run(async () =>
                        {
                            // 更新
                            {
                                // モデル設定ウィンドウが閉じられたときに、MainWindowViewModelのAppSettingsとFoundryLocalFacadeを更新
                                _appSettings = modelSettingWindowViewModel._appSettings;
                                _foundryLocalFacade = modelSettingWindowViewModel._foundryLocalFacade;
                            }

                            // チャットモードの初期化処理
                            {
                                // Catalogの初期化
                                dialogWindowViewModel.Message.Value = "カタログの初期化中...";
                                await _foundryLocalFacade.InitializeCatalogAsync();

                                // モデルのダウンロード
                                dialogWindowViewModel.Message.Value = $"モデルのダウンロード中...\nモデルのサイズによって数十分かかる場合があります。\n\nモデル：{_appSettings.UseModelAlias}";
                                await _foundryLocalFacade.DownloadModelAsync(_appSettings.UseModelAlias);

                                // モデルのロード
                                dialogWindowViewModel.Message.Value = $"モデルの読み込み中...\nモデル：{_appSettings.UseModelAlias}";
                                await _foundryLocalFacade.LoadModelAsync(_appSettings.UseModelAlias);

                                // チャットモードの初期化
                                dialogWindowViewModel.Message.Value = "チャットモードの初期化中...";
                                await _foundryLocalFacade.InitializeChatModeAsync();

                                dialogWindowViewModel.Message.Value = "チャットモードの初期化が完了しました。";

                                // ChatControlViewModel に更新された FoundryLocalFacade を設定
                                ChatControlViewModel.Value.SetService(_foundryLocalFacade);
                            }
                        });
                    }
                    finally
                    {
                        // ダイアログを閉じる
                        dialogWindow.Close();
                    }

                    // ChatControlViewModel の送信メッセージテキストボックスを有効化 
                    ChatControlViewModel.Value.IsEnableSendMessageTextBox.Value = true;
                    ChatControlViewModel.Value.IsEnableSendButton.Value = true;

                    // モデル設定ウィンドウが閉じられた後の処理をここに追加
                    IsEnableTestButton.Value = true;
                };
            }

            // モデル設定ウィンドウを表示
            modelSettingWindow.ShowDialog();
        }

        #endregion << Window >>
    }
}
