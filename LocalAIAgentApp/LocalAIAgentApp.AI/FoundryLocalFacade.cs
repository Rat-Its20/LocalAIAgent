using LocalAIAgentApp.AI.Services;
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;

namespace LocalAIAgentApp.AI
{
    /// <summary>
    /// FoundryLocalFacade クラスは、FoundryLocalManager クラスの機能を統合し、アプリケーション全体で使用できるようにするためのファサードクラスです。
    /// </summary>
    public class FoundryLocalFacade
    {
        #region << Service >>

        /// <summary>
        /// Service：CacheService は、キャッシュ関連の機能を提供するサービスです。
        /// </summary>
        private CacheService _cacheService { get; set; } = new CacheService();

        #endregion << Service >>

        /// <summary>
        /// Constructor
        /// </summary>
        public FoundryLocalFacade()
        {
        }

        /// <summary>
        /// TestChat メソッドは、FoundryLocalManager を使用して、チャットクライアントを取得し、チャットの完了をテストするためのメソッドです。
        /// </summary>
        /// <param name="modelPath"></param>
        /// <returns></returns>
        public async Task TestChat(string modelPath)
        {
            // モデルの保存先のディレクトリが存在しない場合は、処理を終了します。
            if (!Directory.Exists(modelPath))
            {
                return;
            }

            // FoundryLocalManager を初期化します。
            await FoundryLocalManager.CreateAsync(new Configuration
            {
                AppName = _cacheService.CACHE_NAME,
                AppDataDir = modelPath,
            },
            NullLogger.Instance);

            // FoundryLocalManager のインスタンスを使用して、必要な操作を行います。
            using (FoundryLocalManager manager = FoundryLocalManager.Instance)
            {
                // カタログクライアントを取得します。
                ICatalog catalog = await manager.GetCatalogAsync();

                // モデルをカタログに追加します。
                var modelDef = await catalog.GetModelAsync("phi-4-mini");

                // モデルがキャッシュされているかどうかを確認します。
                if (!await modelDef.IsCachedAsync())
                {
                    // モデルをダウンロードします。
                    // どこにダウンロードされるかは、FoundryLocalManager の構成によって異なります。
                    // デフォルトでは、ユーザーのローカルアプリデータフォルダ内の "FoundryLocal" フォルダにダウンロードされます。
                    await modelDef.DownloadAsync();
                }

                // モデルがロードされているかどうかを確認します。 
                if (!await modelDef.IsLoadedAsync())
                {
                    // モデルをロードします。
                    await modelDef.LoadAsync();
                }

                // GetChatClientAsync メソッドを使用して、チャットクライアントを取得します。
                {
                    // チャットクライアントを取得します。
                    OpenAIChatClient chatClient = await modelDef.GetChatClientAsync();
                    
                    string userMessage = string.Empty;
                    string assistantMessage = string.Empty;

                    // ChatMessage のリストを作成し、CompleteChatAsync に渡す
                    List<ChatMessage> messages = new List<ChatMessage>();

                    messages.Add(ChatMessage.FromSystem("あなたは優秀なアシスタントです。"));

                    userMessage = "こんばんは。自己紹介して";
                    messages.Add(ChatMessage.FromUser(userMessage));

                    ChatCompletionCreateResponse response = await chatClient.CompleteChatAsync(messages);
                    assistantMessage = response.Choices.FirstOrDefault().Message.Content;
                    messages.Add(ChatMessage.FromAssistant(assistantMessage));

                    Debug.WriteLine($"assistantMessage: {assistantMessage}");

                    userMessage = "私と仲良くしてくれる？";
                    messages.Add(ChatMessage.FromUser(userMessage));

                    response = await chatClient.CompleteChatAsync(messages);
                    assistantMessage = response.Choices.FirstOrDefault().Message.Content;
                    messages.Add(ChatMessage.FromAssistant(assistantMessage));

                    Debug.WriteLine($"assistantMessage: {assistantMessage}");
                }

                Debug.WriteLine("TestChat method completed successfully.");

                {
                    // TODO：処理が長そうな推論の時は、StreamAsyncを利用することも検討する。
                }
            }

        }
    }
}
