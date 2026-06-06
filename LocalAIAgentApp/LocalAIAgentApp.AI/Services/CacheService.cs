using LocalAIAgentApp.AI.Contracts;

namespace LocalAIAgentApp.AI.Services
{
    internal class CacheService : ICacheService
    {
        /// <summary>
        /// CACHE_NAME はキャッシュの名前を表す定数
        /// </summary>
        private string CACHE_NAME { get; } = "LocalAIAgent";

        /// <summary>
        /// Constructor
        /// </summary>
        public CacheService()
        {
        }

        /// <summary>
        /// GetCachePath メソッドは、キャッシュの保存先のパスを取得するためのメソッドです。
        /// </summary>
        /// <returns></returns>
        public string GetCachePath()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(localAppData, "FoundryLocal", CACHE_NAME);
        }

        /// <summary>
        /// GetCacheSize メソッドは、キャッシュのサイズを取得するためのメソッドです。
        /// </summary>
        /// <returns>キャッシュのサイズ（バイト単位）</returns>
        public long GetCacheSize()
        {
            // キャッシュの保存先のパスを取得します。
            string path = GetCachePath();

            // キャッシュの保存先のディレクトリが存在しない場合は、サイズは 0 とみなします。
            if (!Directory.Exists(path))
            {
                return 0;
            }

            return Directory.GetFiles(path, "*", SearchOption.AllDirectories).Sum(f => new FileInfo(f).Length);
        }

        /// <summary>
        /// ClearCache メソッドは、キャッシュをクリアするためのメソッドです。
        /// </summary>
        public void ClearCache()
        {
            // キャッシュの保存先のパスを取得します。
            string path = GetCachePath();

            // キャッシュの保存先のディレクトリが存在する場合は、ディレクトリを削除します。
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// Exsts メソッドは、キャッシュが存在するかどうかを確認するためのメソッドです。
        /// </summary>
        /// <returns></returns>
        public bool Exsts()
        {
            return Directory.Exists(GetCachePath());
        }
    }
}
