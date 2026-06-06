namespace LocalAIAgentApp.Model.Entity.Json
{
    public class AppSettings
    {
        /// <summary>
        /// IsModel プロパティは、モデルが存在するかどうかを示すフラグを格納するためのプロパティです。
        /// デフォルト値は false です。
        /// </summary>
        public bool IsModel { get; set; } = false;

        /// <summary>
        /// ModelFolderPath プロパティは、モデルのフォルダパスを格納するためのプロパティです。
        /// デフォルト値は空文字列です。
        /// </summary>
        public string ModelFolderPath { get; set; } = string.Empty;
    }
}
