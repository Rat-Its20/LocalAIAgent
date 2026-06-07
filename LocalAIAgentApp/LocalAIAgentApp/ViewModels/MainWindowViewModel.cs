using Autofac;
using LocalAIAgentApp.AI;
using LocalAIAgentApp.Model.Entity.Json;
using LocalAIAgentApp.Model.Services;
using LocalAIAgentApp.Services;
using LocalAIAgentApp.ViewModels.Base;
using LocalAIAgentApp.Views;
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

        #endregion << Property >>

        #region << Command >>

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
            // タイトルバーの左クリックイベントに対応するコマンドの購読を設定
            TitleBarMouseLeftDownCommand.Subscribe(e =>
            {
                // タイトルバーの左クリックイベントを処理するためのメソッドを呼び出す
                OnTitleBarMouseLeftButtonDown(App.Current.MainWindow, e);
            });

            // Window 読み込み時
            LoadCommand.Subscribe(_ =>
            {
                // モデルの初期設定が必要かを判定
                {
                    // モデルフォルダが存在する場合の確認
                    if (!_appSettings.IsCheckModel)
                    {
                        // TODO：モデルの存在確認を行う
                    }
                }

                // モデルの初期設定が必要な場合は、モデル設定ウィンドウを開く
                if (_appSettings.IsCheckModel)
                {
                    // モデル設定ウィンドウを開く
                    OpenModelSettingWindow();
                }
            });

            // Window クローズ前
            ClosingCommand.Subscribe(_ =>
            {
                // FileService を使用して、アプリケーションの設定を保存する。
                {
                    // AppSettings
                    _fileService.SaveAppSettings(_appSettings);
                }
            });

            // ウィンドウを閉じる処理
            CloseCommand.Subscribe(_ =>
            {
                App.Current.Shutdown() ;
            });

            // ウィンドウを最小化する処理
            MinimizeCommand.Subscribe(_ =>
            {
                App.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
            });

            // ウィンドウを最大化/元に戻す処理
            MaximizeCommand.Subscribe(_ =>
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
            ModelSettingWindowViewModel modelSettingWindowViewModel = _lifetimeScope.Resolve<ModelSettingWindowViewModel>();
            ModelSettingWindow modelSettingWindow = _lifetimeScope.Resolve<ModelSettingWindow>();

            // ViewModel関連の処理
            {
                modelSettingWindowViewModel.SetServices(_appSettings, _dialogService, _foundryLocalFacade);
            }

            // Window関連の処理
            {
                modelSettingWindow.Owner = App.Current.MainWindow;
                modelSettingWindow.DataContext = modelSettingWindowViewModel;

                // モデル設定ウィンドウが閉じられたときの処理を追加
                modelSettingWindow.Closing += (sender, e) =>
                {
                    // モデル設定ウィンドウが閉じられたときに、MainWindowViewModelのAppSettingsとFoundryLocalFacadeを更新
                    _appSettings = modelSettingWindowViewModel._appSettings;
                    _foundryLocalFacade = modelSettingWindowViewModel._foundryLocalFacade;
                };
            }

            // モデル設定ウィンドウを表示
            modelSettingWindow.ShowDialog();
        }

        #endregion << Window >>
    }
}
