using LocalAIAgentApp.AI;
using LocalAIAgentApp.Entity;
using LocalAIAgentApp.ViewModels.Base;
using Reactive.Bindings;
using System.Windows.Forms;

namespace LocalAIAgentApp.ViewModels.Pages
{
    public class ChatControlViewModel : ViewModelBase
    {
        /// <summary>
        /// Field：FoundryLocalFacade は、FoundryLocalFacade クラスのインスタンスを格納するためのプロパティです。
        /// </summary>
        private FoundryLocalFacade _foundryLocalFacade { get; set; }

        /// <summary>
        /// test
        /// </summary>
        public ReactiveProperty<string> Test { get; set; } = new ReactiveProperty<string>("TEST");

        #region << Property >>

        /// <summary>
        /// Property：IsEnableSendButton は、送信ボタンが有効かどうかを示すブール値を格納するためのプロパティです。
        /// </summary>
        public ReactiveProperty<bool> IsEnableSendButton { get; set; } = new ReactiveProperty<bool>(false);

        /// <summary>
        /// Collection：Messages は、チャットメッセージのコレクションを格納するためのプロパティです。
        /// </summary>
        public ReactiveCollection<ChatMessageModel> Messages { get; set; } = new ReactiveCollection<ChatMessageModel>();

        /// <summary>
        /// Property：IsEnableSendMessageTextBox は、送信メッセージのテキストボックスが有効かどうかを示すブール値を格納するためのプロパティです。
        /// </summary>
        public ReactiveProperty<bool> IsEnableSendMessageTextBox { get; set; } = new ReactiveProperty<bool>(false);

        /// <summary>
        /// Property：SendMessageTextBox は、送信メッセージのテキストボックスに入力されたテキストを格納するためのプロパティです。
        /// </summary>
        public ReactiveProperty<string> SendMessageTextBox { get; set; } = new ReactiveProperty<string>();

        #endregion << Property >>

        #region << Command >>

        /// <summary>
        /// Command：SendMessageCommand は、メッセージを送信するためのコマンドです。
        /// </summary>
        public ReactiveCommand SendMessageCommand { get; set; } = new ReactiveCommand();

        #endregion << Command >>

        /// <summary>
        /// Constructor
        /// </summary>
        public ChatControlViewModel()
        {
            SetSubscribe();
        }

        /// <summary>
        /// SetService メソッドは、FoundryLocalFacade クラスのインスタンスを受け取り、ChatControlViewModel 内で使用できるようにするためのメソッドです。
        /// </summary>
        /// <param name="foundryLocalFacade"></param>
        public void SetService(FoundryLocalFacade foundryLocalFacade)
        {
            _foundryLocalFacade = foundryLocalFacade;
        }

        public override void Initialize()
        {
        }

        public override void SetSubscribe()
        {
            // 送信メッセージのテキストボックスに入力されたテキストが変更されたときの処理を定義します。
            SendMessageCommand.Subscribe(async() =>
            {
                // 送信メッセージのテキストボックスに入力されたテキストが空でないかどうかを確認します。
                if (string.IsNullOrEmpty(SendMessageTextBox.Value))
                {
                    return;
                }

                // メッセージをコレクションに追加します。
                Messages.Add(new ChatMessageModel { Text = $"USER：{SendMessageTextBox.Value}", IsUser = true });

                // 送信メッセージのテキストボックスを空にします。
                SendMessageTextBox.Value = string.Empty;

                try
                {
                    // 送信ボタンと送信メッセージのテキストボックスを無効にします。
                    IsEnableSendButton.Value = false;
                    IsEnableSendMessageTextBox.Value = false;

                    // メッセージを送信する処理を非同期で実行します。
                    Messages.Add(new ChatMessageModel { Text = $"ASSISTANT：{await _foundryLocalFacade.Talk(SendMessageTextBox.Value)}", IsUser = false });
                }
                finally
                {
                    // 送信ボタンと送信メッセージのテキストボックスを有効にします。
                    IsEnableSendButton.Value = true;
                    IsEnableSendMessageTextBox.Value = true;
                }
            });
        }
    }
}
