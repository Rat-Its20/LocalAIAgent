namespace LocalAIAgentApp.Model.Entity.Json
{
    public class AppSettings
    {
        /// <summary>
        /// IsCheckModel プロパティは、モデルのチェックを行うかどうかを示すブール値を格納するためのプロパティです。
        /// デフォルト値は true です。
        /// </summary>
        public bool IsCheckModel { get; set; } = true;

        /// <summary>
        /// WorkDirectoryPath プロパティは、モデルのフォルダパスを格納するためのプロパティです。
        /// デフォルト値は空文字列です。
        /// </summary>
        public string WorkDirectoryPath { get; set; } = string.Empty;

        /// <summary>
        /// UseModelAlias プロパティは、モデルのエイリアスを使用するかどうかを示す文字列を格納するためのプロパティです。
        /// デフォルト値は空文字列です。
        /// </summary>
        public string UseModelAlias { get; set; } = string.Empty;
    }
}
