using LocalAIAgentApp.AI;
using LocalAIAgentApp.Model.Entity.Json;
using LocalAIAgentApp.Services;
using LocalAIAgentApp.ViewModels.Base;
using LocalAIAgentApp.Views;
using Reactive.Bindings;
using System.IO;
using System.Windows;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace LocalAIAgentApp.ViewModels
{
    public class ModelSettingWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Field：AppSettings は、アプリケーションの設定を格納するためのプロパティです。
        /// </summary>
        public AppSettings _appSettings { get; private set; }

        /// <summary>
        /// Field：FoundryLocalFacade は、FoundryLocalFacade クラスのインスタンスを格納するためのプロパティです。
        /// </summary>
        public FoundryLocalFacade _foundryLocalFacade { get; set; }

        #region << Service >>

        /// <summary>
        /// Service：DialogService
        /// </summary>
        private DialogService _dialogService { get; set; }

        #endregion << Service >>

        #region << Property >>

        /// <summary>
        /// Property：ModelPath は、ユーザーが選択したモデルのパスを格納するためのプロパティです。
        /// </summary>
        public ReactiveProperty<string> ModelPath { get; set; } = new ReactiveProperty<string>();

        #endregion << Property >>

        #region << Command >>

        /// <summary>
        /// 参照コマンド：ReferenceCommand は、ユーザーが参照ボタンをクリックしたときに実行されるコマンドです。
        /// </summary>
        public ReactiveCommand ReferenceCommand { get; set; } = new ReactiveCommand();

        /// <summary>
        /// 保存コマンド：SaveCommand は、ユーザーが保存ボタンをクリックしたときに実行されるコマンドです。
        /// </summary>
        public ReactiveCommand SaveCommand { get; set; } = new ReactiveCommand();

        /// <summary>
        /// 閉じるコマンド：CloseCommand は、ユーザーが閉じるボタンをクリックしたときに実行されるコマンドです。
        /// </summary>
        public ReactiveCommand CloseCommand { get; set; } = new ReactiveCommand();

        #endregion << Command >>

        /// <summary>
        /// Constructor
        /// </summary>
        public ModelSettingWindowViewModel() : base()
        {
            SetSubscribe();
        }

        /// <summary>
        /// Initialize メソッドは、ViewModel の初期化処理を行うためのメソッドです。
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// SetSubscribe メソッドは、ViewModel のイベントやコマンドの購読を設定するためのメソッドです。
        /// </summary>
        public override void SetSubscribe()
        {
            // 参照コマンドの購読を設定
            ReferenceCommand.Subscribe(() =>
            {
                // ダイアログを開いてフォルダを選択。
                string result = _dialogService.OpenFolderDialog(ModelPath.Value);

                // 選択されたフォルダが存在する場合は、ModelPath プロパティにそのパスを設定
                if (Directory.Exists(result))
                {
                    ModelPath.Value = result;
                }
            });

            // 保存コマンドの購読を設定
            SaveCommand.Subscribe(() =>
            {
                // ValidateChecker メソッドを呼び出して、ユーザーが入力した値の検証を行います。
                if (ValidateChecker())
                {
                    // ModelPath プロパティの値を AppSettings の ModelFolderPath に保存
                    _appSettings.ModelFolderPath = ModelPath.Value;

                    MessageBox.Show("保存しました。", "保存", MessageBoxButton.OK, MessageBoxImage.Information);

                    // ウィンドウを閉じる
                    WindowClose();
                }
            });

            // 閉じるコマンドの購読を設定
            CloseCommand.Subscribe(() =>
            {
                // ウィンドウを閉じる
                WindowClose();
            });
        }

        /// <summary>
        /// SetServices メソッドは、ViewModel に必要なサービスを設定するためのメソッドです。
        /// </summary>
        /// <param name="appSettings">アプリケーションの設定を格納する AppSettings インスタンス</param>
        /// <param name="dialogService">DialogService インスタンス</param>
        /// <param name="foundryLocalFacade">FoundryLocalFacade インスタンス</param>
        internal void SetServices(AppSettings appSettings, DialogService dialogService, FoundryLocalFacade foundryLocalFacade)
        {
            // Serviceを設定
            {
                _appSettings = appSettings;
                _dialogService = dialogService;
                _foundryLocalFacade = foundryLocalFacade;
            }

            {
                // ModelPath プロパティに AppSettings の ModelFolderPath の値を設定
                ModelPath.Value = _appSettings.ModelFolderPath;
            }
        }

        /// <summary>
        /// ValidateChecker メソッドは、ユーザーが入力した値の検証を行うためのメソッドです。
        /// </summary>
        /// <returns></returns>
        private bool ValidateChecker()
        {
            // TODO：ModelPath プロパティの値が有効なフォルダパスであることを検証するロジックを実装する必要があります。
            bool isValid = true;

            {
                 if (string.IsNullOrEmpty(ModelPath.Value) || !Directory.Exists(ModelPath.Value))
                {
                    isValid = false;
                    MessageBox.Show("モデルの読み込み先が無効です。\n有効なモデルのフォルダパスを入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return isValid;
        }

        /// <summary>
        /// WindowClose メソッドは、ウィンドウを閉じるためのメソッドです。
        /// </summary>
        private void WindowClose()
        {
            Application.Current.Windows.OfType<ModelSettingWindow>().FirstOrDefault()?.Close();
        }
    }
}
