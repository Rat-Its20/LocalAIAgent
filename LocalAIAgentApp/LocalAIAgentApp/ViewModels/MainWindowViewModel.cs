using Autofac;
using LocalAIAgentApp.Model.Entity.Json;
using LocalAIAgentApp.Model.Services;
using LocalAIAgentApp.ViewModels.Base;
using Reactive.Bindings;
using System.Diagnostics;

namespace LocalAIAgentApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Field：AppSettings は、アプリケーションの設定を格納するためのプロパティです。
        /// </summary>
        private AppSettings _appSettings { get; set; }

        /// <summary>
        /// Service：FileService
        /// </summary>
        private FileService _fileService { get; set; }

        #region << Property >>
        /// <summary>
        /// test
        /// </summary>
        public ReactiveProperty<string> Test { get; set; } = new ReactiveProperty<string>("TEST");
        #endregion << Property >>

        #region << Command >>

        /// <summary>
        /// WindowLoadCommand は、ウィンドウのロードイベントに対応するコマンドです。
        /// </summary>
        public ReactiveCommand LoadCommand { get; set; } = new ReactiveCommand();

        /// <summary>
        /// WindowClosingCommand は、ウィンドウのクローズイベントに対応するコマンドです。
        /// </summary>
        public ReactiveCommand ClosingCommand { get; set; } = new ReactiveCommand();

        #endregion << Command >>

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel(ILifetimeScope lifetimeScope, FileService fileService) : base()
        {
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
            // Window 読み込み時
            LoadCommand.Subscribe(_ =>
            {
                // モデルが存在しない場合の処理
                {
                    if (!_appSettings.IsModel)
                    {

                    }
                }
            });

            // Window クローズ時
            ClosingCommand.Subscribe(_ =>
            {
                // FileService を使用して、アプリケーションの設定を保存する。
                {
                    // AppSettings
                    _fileService.SaveAppSettings(_appSettings);
                }
            });
        }
    }
}
