using LocalAIAgentApp.Model.Entity.Json;
using LocalAIAgentApp.Model.Services;
using LocalAIAgentApp.ViewModels.Base;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel(FileService fileService) : base()
        {
            // FileService 
            {
                _fileService = fileService;

                // 初期化処理：ファイルの存在確認と AppSettings の読み込み
                _appSettings = _fileService.InitializeFolderConfigure(AppDomain.CurrentDomain.BaseDirectory);
            }
        }

        public override void Initialize()
        {
            // Initialize logic here
        }

        public override void SetSubscribe()
        {
            // SetSubscribe logic here
        }
    }
}
