using LocalAIAgentApp.AI;
using LocalAIAgentApp.Model.Entity.Json;
using LocalAIAgentApp.Model.Services;
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
    public class ModelSettingWindowViewModel : WindowViewModelBase
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

        /// <summary>
        /// Service：FileService
        /// </summary>
        private FileService _fileService { get; set; }

        #endregion << Service >>

        #region << Property >>

        /// <summary>
        /// Property：WorkDirectory は、ユーザーが選択した作業ディレクトリのパスを格納するためのプロパティです。
        /// </summary>
        public ReactiveProperty<string> WorkDirectory { get; set; } = new ReactiveProperty<string>();

        /// <summary>
        /// Property：IsEnableModelSettingGroup は、モデル設定グループの有効/無効を示すブール値を格納するためのプロパティです。
        /// デフォルト値は false です。
        /// </summary>
        public ReactiveProperty<bool> IsEnableModelSettingGroup { get; set; } = new ReactiveProperty<bool>(false);

        /// <summary>
        /// Property：IsEnableModelAliasCollection は、モデルエイリアスのコレクションの有効/無効を示すブール値を格納するためのプロパティです。
        /// デフォルト値は false です。
        /// </summary>
        public ReactiveProperty<bool> IsEnableModelAliasCollection { get; set; } = new ReactiveProperty<bool>(false);

        /// <summary>
        /// Collection：ModelAliasCollection は、モデルのエイリアスのコレクションを格納するためのプロパティです。
        /// </summary>
        public ReactiveCollection<string> ModelAliasCollection { get; set; } = new ReactiveCollection<string>();

        /// <summary>
        /// Property：SelectedModelAlias は、ユーザーが選択したモデルのエイリアスを格納するためのプロパティです。
        /// </summary>
        public ReactiveProperty<string> SelectedModelAlias { get; set; } = new ReactiveProperty<string>();

        /// <summary>
        /// Property：IsEnableReferenceButton は、参照ボタンの有効/無効を示すブール値を格納するためのプロパティです。
        /// デフォルト値は true です。
        /// </summary>
        public ReactiveProperty<bool> IsEnableReferenceButton { get; set; } = new ReactiveProperty<bool>(true); 

        #endregion << Property >>

        #region << Command >>

        /// <summary>
        /// 参照コマンド：ReferenceCommand は、ユーザーが参照ボタンをクリックしたときに実行されるコマンドです。
        /// </summary>
        public ReactiveCommand ReferenceCommand { get; set; } = new ReactiveCommand();

        /// <summary>
        /// 読込コマンド：AliasLoadCommand は、ユーザーが読込ボタンをクリックしたときに実行されるコマンドです。
        /// </summary>
        public ReactiveCommand AliasLoadCommand { get; set; } = new ReactiveCommand();

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
            // タイトルバーの左クリックイベントの購読を設定
            TitleBarMouseLeftDownCommand.Subscribe(e =>
            {
                // タイトルバーの左クリックイベントが発生したときに、ウィンドウをドラッグして移動できるようにする処理
                OnTitleBarMouseLeftButtonDown(Application.Current.Windows.OfType<ModelSettingWindow>().FirstOrDefault(), e);
            });

            // 参照コマンドの購読を設定
            ReferenceCommand.Subscribe(() =>
            {
                // ダイアログを開いてフォルダを選択。
                string result = _dialogService.OpenFolderDialog(WorkDirectory.Value);

                // 選択されたフォルダが存在する場合は、WorkDirectory プロパティにそのパスを設定
                if (Directory.Exists(result))
                {
                    // WorkDirectory プロパティに選択されたフォルダのパスを設定
                    WorkDirectory.Value = result;

                    // モデル設定グループを有効にするために、IsEnableModelSettingGroup プロパティを true に設定
                    IsEnableModelSettingGroup.Value = true;
                }
            });

            // 読込コマンドの購読を設定
            AliasLoadCommand.Subscribe(async() =>
            {
                // Enable制御
                {
                    // モデルエイリアスのコレクションを一時的に無効化して、ユーザーがモデルのエイリアスのリストを更新する前に、モデルのエイリアスのコレクションを操作できないようにします。
                    IsEnableModelAliasCollection.Value = false;

                    // IsEnableReferenceButton プロパティを false に設定して、ユーザーが参照ボタンをクリックできないようにします。
                    IsEnableReferenceButton.Value = false;
                }

                // WorkDirectory プロパティの値が有効なフォルダパスであることを検証するロジックを実装する必要があります。
                if (!_foundryLocalFacade.isInitialized)
                {
                    await _foundryLocalFacade.CreateManager(_appSettings.WorkDirectoryPath);
                }

                // SelectedModelAlias を初期化し、ModelAliasCollection をクリアしてから、FoundryLocalFacade を使用してモデルのエイリアスのリストを取得し、ModelAliasCollection に追加します。
                SelectedModelAlias.Value = string.Empty;
                ModelAliasCollection.Clear();

                // FoundryLocalFacade を使用してモデルのエイリアスのリストを取得し、ModelAliasCollection に追加します。
                ModelAliasCollection.AddRangeOnScheduler(await _foundryLocalFacade.GetAliasList());

                // AppSettings の UseModelAlias の値を SelectedModelAlias に設定します。
                SelectedModelAlias.Value = _appSettings.UseModelAlias;

                // Enable制御
                {
                    // モデルエイリアスのコレクションを有効化
                    IsEnableModelAliasCollection.Value = true;

                    // 参照ボタンを再度有効にします。
                    IsEnableReferenceButton.Value = true;
                }
            });

            // 保存コマンドの購読を設定
            SaveCommand.Subscribe(() =>
            {
                // ValidateChecker メソッドを呼び出して、ユーザーが入力した値の検証を行います。
                if (ValidateChecker())
                {
                    // appSettings のプロパティに、ユーザーが入力した値を保存します。
                    {
                        // WorkDirectory プロパティの値を AppSettings の WorkDirectoryPath に保存
                        _appSettings.WorkDirectoryPath = WorkDirectory.Value;

                        // SelectedModelAlias プロパティの値を AppSettings の UseModelAlias に保存
                        _appSettings.UseModelAlias = SelectedModelAlias.Value;
                    }

                    // AppSettings を保存
                    _fileService.SaveAppSettings(_appSettings);

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
        /// <param name="fileService">FileService インスタンス</param>
        /// <param name="foundryLocalFacade">FoundryLocalFacade インスタンス</param>
        internal void SetServices(AppSettings appSettings, DialogService dialogService, FileService fileService, FoundryLocalFacade foundryLocalFacade)
        {
            // Serviceを設定
            {
                _appSettings = appSettings;
                _dialogService = dialogService;
                _fileService = fileService;
                _foundryLocalFacade = foundryLocalFacade;
            }

            // 初期化
            {
                // ModelAliasCollection をクリア
                ModelAliasCollection.Clear();

                // SelectedModelAlias を初期化
                SelectedModelAlias.Value = _appSettings.UseModelAlias;

                // WorkDirectory プロパティに AppSettings の WorkDirectoryPath の値を設定
                WorkDirectory.Value = _appSettings.WorkDirectoryPath;
            }
        }

        /// <summary>
        /// ValidateChecker メソッドは、ユーザーが入力した値の検証を行うためのメソッドです。
        /// </summary>
        /// <returns></returns>
        private bool ValidateChecker()
        {
            // TODO：WorkDirectory プロパティの値が有効なフォルダパスであることを検証するロジックを実装する必要があります。
            bool isValid = true;

            // WorkDirectory プロパティの値が有効なフォルダパスであることを検証
            {
                if (string.IsNullOrEmpty(WorkDirectory.Value) || !Directory.Exists(WorkDirectory.Value))
                {
                    isValid = false;
                    MessageBox.Show("作業ディレクトリが無効です。\n有効な作業ディレクトリのパスを入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
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
