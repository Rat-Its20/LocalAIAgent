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

        /// <summary>
        /// Field：catalog は、ICatalog インターフェイスのインスタンスを格納するためのプロパティです。
        /// </summary>
        private ICatalog _catalog { get; set; }

        /// <summary>
        /// Field：modelDef は、IModel インターフェイスのインスタンスを格納するためのプロパティです。
        /// </summary>
        private IModel _modelDef { get; set; }

        /// <summary>
        /// Field：chatClient は、OpenAIChatClient クラスのインスタンスを格納するためのプロパティです。
        /// </summary>
        private OpenAIChatClient _chatClient { get; set; }

        /// <summary>
        /// Field：Messages は、ChatMessage クラスのインスタンスのリストを格納するためのプロパティです。
        /// </summary>
        public List<ChatMessage> _messages { get; set; } = new List<ChatMessage>();

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

            // カタログクライアントを取得します。
            if (_catalog == null)
            {
                _catalog = await GetCatalogAsync();
            }

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
            if (_catalog == null)
            {
                _catalog = await GetCatalogAsync();
            }

            // カタログ内のモデルをリストし、そのエイリアスをリストにします。
            result = (await _catalog.ListModelsAsync()).Select(m => m.Alias).ToList();

            return result;
        }

        /// <summary>
        /// InitializeCatalogAsync メソッドは、カタログクライアントを初期化するためのメソッドです。
        /// </summary>
        /// <returns></returns>
        public async Task InitializeCatalogAsync()
        {
            // カタログクライアントを取得します。
            if (_catalog == null)
            {
                _catalog = await GetCatalogAsync();
            }
        }

        /// <summary>
        /// DownloadModelAsync メソッドは、指定されたモデルのエイリアスに基づいて、モデルをダウンロードするためのメソッドです。
        /// </summary>
        /// <param name="modelAlias">モデルのエイリアス</param>
        /// <returns></returns>
        public async Task DownloadModelAsync(string modelAlias)
        {
            // TODO：モデルのエイリアスが存在するかどうかを確認する処理を追加することも検討する。
            if (!modelAlias.Equals("phi-4-mini"))
            {
                return;
            }

            // TODO：例外処理が甘いので、モデルのエイリアスが存在しない場合や、ダウンロードやロードに失敗した場合の例外処理を追加することも検討する。

            // モデルをカタログに追加します。
            _modelDef = await _catalog.GetModelAsync(modelAlias);

            // モデルがキャッシュされているかどうかを確認します。
            if (!await _modelDef.IsCachedAsync())
            {
                // モデルをダウンロードします。
                // どこにダウンロードされるかは、FoundryLocalManager の構成によって異なります。
                // デフォルトでは、ユーザーのローカルアプリデータフォルダ内の "FoundryLocal" フォルダにダウンロードされます。
                await _modelDef.DownloadAsync();
            }
        }

        /// <summary>
        /// LoadModelAsync メソッドは、指定されたモデルのエイリアスに基づいて、モデルをロードするためのメソッドです。
        /// </summary>
        /// <param name="modelAlias">モデルのエイリアス</param>
        /// <returns></returns>
        public async Task LoadModelAsync(string modelAlias)
        {
            // TODO：モデルのエイリアスが存在するかどうかを確認する処理を追加することも検討する。
            if (!modelAlias.Equals("phi-4-mini"))
            {
                return;
            }

            // TODO：例外処理が甘いので、モデルのエイリアスが存在しない場合や、ダウンロードやロードに失敗した場合の例外処理を追加することも検討する。

            // モデルがロードされているかどうかを確認します。 
            if (!await _modelDef.IsLoadedAsync())
            {
                // モデルをロードします。
                await _modelDef.LoadAsync();
            }
        }

        /// <summary>
        /// InitializeChatModeAsync メソッドは、チャットクライアントを初期化するためのメソッドです。
        /// </summary>
        /// <returns></returns>
        public async Task InitializeChatModeAsync()
        {
            // チャットクライアントを取得します。
            _chatClient = await _modelDef.GetChatClientAsync();

            // Messages を初期化します。
            _messages = new List<ChatMessage>();
            _messages.Add(ChatMessage.FromSystem("あなたは優秀なアシスタントです。"));
        }

        /// <summary>
        /// Talk メソッドは、ユーザーメッセージを受け取り、チャットクライアントを使用して応答を生成し、その応答を返すためのメソッドです。
        /// </summary>
        /// <param name="userMessage">ユーザーメッセージ</param>
        /// <returns>アシスタントの応答メッセージ</returns>
        public async Task<string> Talk(string userMessage)
        {
            string assistantMessage = string.Empty;

            _messages.Add(ChatMessage.FromUser(userMessage));

            ChatCompletionCreateResponse response = await _chatClient.CompleteChatAsync(_messages);
            assistantMessage = response.Choices.FirstOrDefault().Message.Content;

            _messages.Add(ChatMessage.FromAssistant(assistantMessage));

            return assistantMessage;
        }

        /// <summary>
        /// TestChat メソッドは、FoundryLocalManager を使用して、チャットクライアントを取得し、チャットの完了をテストするためのメソッドです。
        /// </summary>
        /// <returns></returns>
        public async Task TestChat()
        {
            // GetChatClientAsync メソッドを使用して、チャットクライアントを取得します。
            {
                string userMessage = string.Empty;
                string assistantMessage = string.Empty;

                // ChatMessage のリストを作成し、CompleteChatAsync に渡す
                List<ChatMessage> messages = new List<ChatMessage>();

                messages.Add(ChatMessage.FromSystem("あなたは優秀なアシスタントです。"));

                userMessage = "こんばんは。自己紹介して";
                messages.Add(ChatMessage.FromUser(userMessage));

                ChatCompletionCreateResponse response = await _chatClient.CompleteChatAsync(messages);
                assistantMessage = response.Choices.FirstOrDefault().Message.Content;
                messages.Add(ChatMessage.FromAssistant(assistantMessage));

                Debug.WriteLine($"assistantMessage: {assistantMessage}");

                userMessage = "私と仲良くしてくれる？";
                messages.Add(ChatMessage.FromUser(userMessage));

                response = await _chatClient.CompleteChatAsync(messages);
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
