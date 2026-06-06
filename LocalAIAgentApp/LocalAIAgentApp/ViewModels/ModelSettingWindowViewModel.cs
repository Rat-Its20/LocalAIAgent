using LocalAIAgentApp.AI;
using LocalAIAgentApp.Model.Entity.Json;
using LocalAIAgentApp.ViewModels.Base;

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
        public FoundryLocalFacade _foundryLocalFacade { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ModelSettingWindowViewModel()
        {
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
        }

        /// <summary>
        /// SetServices メソッドは、ViewModel に必要なサービスを設定するためのメソッドです。
        /// </summary>
        /// <param name="appSettings">アプリケーションの設定を格納する AppSettings インスタンス</param>
        /// <param name="foundryLocalFacade">FoundryLocalFacade インスタンス</param>
        public void SetServices(AppSettings appSettings, FoundryLocalFacade foundryLocalFacade)
        {
            _appSettings = appSettings;
            _foundryLocalFacade = foundryLocalFacade;
        }
    }
}
