namespace LocalAIAgentApp.AI.Contracts
{
    internal interface ICacheService
    {
        /// <summary>
        /// GetCachePath メソッドは、キャッシュの保存先のパスを取得するためのメソッドです。
        /// </summary>
        /// <returns></returns>
        string GetCachePath();

        /// <summary>
        /// GetCacheSize メソッドは、キャッシュのサイズを取得するためのメソッドです。
        /// </summary>
        /// <returns></returns>
        long GetCacheSize();

        /// <summary>
        /// ClearCache メソッドは、キャッシュをクリアするためのメソッドです。
        /// </summary>
        void ClearCache();

        /// <summary>
        /// Exsts メソッドは、キャッシュが存在するかどうかを確認するためのメソッドです。
        /// </summary>
        /// <returns></returns>
        bool Exsts();
    }
}
