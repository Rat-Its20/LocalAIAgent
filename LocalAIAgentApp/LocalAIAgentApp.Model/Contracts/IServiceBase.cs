namespace LocalAIAgentApp.Model.Contracts
{
    public interface IServiceBase : IDisposable
    {
        /// <summary>
        /// Initialize メソッドは、サービスの初期化を行うためのメソッドです。
        /// </summary>
        void Initialize();
    }
}
