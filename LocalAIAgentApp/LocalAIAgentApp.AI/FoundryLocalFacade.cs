using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;
using LocalAIAgentApp.AI.Services;
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;

namespace LocalAIAgentApp.AI
{
    /// <summary>
    /// FoundryLocalFacade クラスは、FoundryLocalManager クラスの機能を統合し、アプリケーション全体で使用できるようにするためのファサードクラスです。
    /// </summary>
    public class FoundryLocalFacade : IDisposable
    {
        /// <summary>
        /// Field：isInitialized は、FoundryLocalManager が初期化されているかどうかを示すブール値を格納するためのプロパティです。
        /// デフォルト値は false です。
        /// </summary>
        public bool isInitialized { get; private set; } = false;

        /// <summary>
        /// Field：FoundryLocalManager は、FoundryLocalManager クラスのインスタンスを格納するためのプロパティです。
        /// </summary>
        private FoundryLocalManager _foundryLocalManager { get; set; }

        #region << Service >>

        /// <summary>
        /// Service：CacheService は、キャッシュ関連の機能を提供するサービスです。
        /// </summary>
        private CacheService _cacheService { get; set; } = new CacheService();

        #endregion << Service >>

        #region << Constructor >>

        /// <summary>
        /// Constructor
        /// </summary>
        public FoundryLocalFacade()
        {
        }

        #endregion << Constructor >>

        /// <summary>
        /// CreateManager メソッドは、FoundryLocalManager を初期化し、そのインスタンスを取得するためのメソッドです。
        /// </summary>
        /// <param name="workPath">作業ディレクトリのパス</param>
        public async Task CreateManager(string workPath)
        {
            if (!Directory.Exists(workPath))
            {
                isInitialized = false;
                return;
            }

            // FoundryLocalManager を初期化します。
            await FoundryLocalManager.CreateAsync(new Configuration
            {
                AppName = _cacheService.CACHE_NAME,
                AppDataDir = workPath,
            },
            NullLogger.Instance);

            // FoundryLocalManager のインスタンスを取得します。
            _foundryLocalManager = FoundryLocalManager.Instance;

            // isInitialized を true に設定します。
            isInitialized = true;
        }

        /// <summary>
        /// GetCatalogAsync メソッドは、FoundryLocalManager を使用して、カタログクライアントを非同期に取得するためのメソッドです。
        /// </summary>
        /// <returns></returns>
        private async Task<ICatalog> GetCatalogAsync()
        {
            if (!isInitialized)
            {
                return null;
            }

            return await _foundryLocalManager.GetCatalogAsync();
        }

        /// <summary>
        /// GetAliasList メソッドは、FoundryLocalManager を使用して、利用可能なモデルのエイリアスのリストを取得するためのメソッドです。
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetAliasList()
        {
            // 初期化
            List<string> result = new List<string>();

            // FoundryLocalManager が初期化されていない場合は、空のリストを返します。
            if (!isInitialized)
            {
                return result;
            }

            // カタログクライアントを取得します。
            ICatalog catalog = await GetCatalogAsync();

            // カタログ内のモデルをリストし、そのエイリアスをリストにします。
            result = (await catalog.ListModelsAsync()).Select(m => m.Alias).ToList();

            return result;
        }

        /// <summary>
        /// TestChat メソッドは、FoundryLocalManager を使用して、チャットクライアントを取得し、チャットの完了をテストするためのメソッドです。
        /// </summary>
        /// <returns></returns>
        public async Task TestChat()
        {
            // カタログクライアントを取得します。
            ICatalog catalog = await GetCatalogAsync();

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

        #region << Dispose >>

        /// <summary>
        /// Dispose メソッドは、FoundryLocalFacade クラスのリソースを解放するためのメソッドです。
        /// </summary>
        public void Dispose()
        {
            _foundryLocalManager?.Dispose();
        }

        #endregion << Dispose >>
    }
}
