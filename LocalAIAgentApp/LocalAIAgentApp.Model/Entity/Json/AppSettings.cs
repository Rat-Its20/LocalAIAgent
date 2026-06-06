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
        /// ModelFolderPath プロパティは、モデルのフォルダパスを格納するためのプロパティです。
        /// デフォルト値は空文字列です。
        /// </summary>
        public string ModelFolderPath { get; set; } = string.Empty;
    }
}
