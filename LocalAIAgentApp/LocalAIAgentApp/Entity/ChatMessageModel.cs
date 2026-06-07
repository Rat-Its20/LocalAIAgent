namespace LocalAIAgentApp.Entity
{
    public class ChatMessageModel
    {
        /// <summary>
        /// Text プロパティは、チャットメッセージの内容を格納するためのプロパティです。
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// IsUser プロパティは、チャットメッセージがユーザーからのものであるかどうかを示すブール値を格納するためのプロパティです。
        /// </summary>
        public bool IsUser { get; set; }
    }
}
