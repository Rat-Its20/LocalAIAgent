using LocalAIAgentApp.Model.Contracts;

namespace LocalAIAgentApp.Model.Base
{
    public abstract class ServiceBase : IServiceBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceBase()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize メソッドは、サービスの初期化を行うためのメソッドです。
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Dispose メソッドは、リソースの解放を行うためのメソッドです。
        /// </summary>
        public virtual void Dispose()
        {
            // デフォルトのリソース解放処理をここに記述します。
        }
    }
}
