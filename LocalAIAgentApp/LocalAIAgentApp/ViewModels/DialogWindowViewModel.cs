using LocalAIAgentApp.ViewModels.Base;
using Reactive.Bindings;

namespace LocalAIAgentApp.ViewModels
{
    public class DialogWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Property：Message は、ダイアログウィンドウに表示するメッセージを格納するためのプロパティです。
        /// </summary>
        public ReactiveProperty<string> Message { get; set; } = new ReactiveProperty<string>();

        public override void Initialize()
        {
        }

        public override void SetSubscribe()
        {
        }
    }
}
