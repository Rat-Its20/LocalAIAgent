namespace LocalAIAgentApp.ViewModels.Base
{
    interface IViewModelBase
    {
        /// <summary>
        /// Initialize メソッドは、ViewModel の初期化処理を行うためのメソッドです。
        /// </summary>
        void Initialize();

        /// <summary>
        /// SetSubscribe メソッドは、ViewModel が必要とするイベントやプロパティの購読を設定するためのメソッドです。
        /// </summary>
        void SetSubscribe();
    }
}
