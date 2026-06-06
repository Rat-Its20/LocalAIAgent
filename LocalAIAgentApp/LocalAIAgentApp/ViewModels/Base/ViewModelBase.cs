using LocalAIAgentApp.Contracts;

namespace LocalAIAgentApp.ViewModels.Base
{
    public abstract class ViewModelBase : IViewModelBase
    {
        /// <summary>
        /// ConstructorCompleted イベントは、ViewModel のコンストラクタの処理が完了した際に発生するイベントです。
        /// </summary>
        public event EventHandler ConstructorCompleted;

        /// <summary>
        /// Constructor メソッドは、ViewModel のインスタンスが生成された際に呼び出されるコンストラクタです。
        /// </summary>
        public ViewModelBase()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize メソッドは、ViewModel の初期化処理を行うためのメソッドです。
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// SetSubscribe メソッドは、ViewModel が必要とするイベントの購読や、データのバインディングなどの設定を行うためのメソッドです。
        /// </summary>
        public abstract void SetSubscribe();
    }
}
