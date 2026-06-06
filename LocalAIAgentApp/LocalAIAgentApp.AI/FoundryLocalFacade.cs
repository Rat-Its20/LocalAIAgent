using LocalAIAgentApp.AI.Services;

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
    }
}
