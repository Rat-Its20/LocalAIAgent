using LocalAIAgentApp.Model.Entity.Json;
using LocalAIAgentApp.ViewModels.Base;

namespace LocalAIAgentApp.ViewModels
{
    public class ModelSettingWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Field：AppSettings は、アプリケーションの設定を格納するためのプロパティです。
        /// </summary>
        public AppSettings AppSettings { get; set; }

        public override void Initialize()
        {
        }

        public override void SetSubscribe()
        {
        }
    }
}
