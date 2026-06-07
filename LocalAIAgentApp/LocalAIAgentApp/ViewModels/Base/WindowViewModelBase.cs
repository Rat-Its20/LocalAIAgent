using Reactive.Bindings;
using System.Windows;
using System.Windows.Input;

namespace LocalAIAgentApp.ViewModels.Base
{
    public abstract class WindowViewModelBase : ViewModelBase
    {
        /// <summary>
        /// TitleBarMouseLeftDownCommand は、タイトルバーの左クリックイベントに対応するコマンドです。
        /// </summary>
        public ReactiveCommand<MouseButtonEventArgs> TitleBarMouseLeftDownCommand { get; set; } = new ReactiveCommand<MouseButtonEventArgs>();

        /// <summary>
        /// OnTitleBarMouseLeftButtonDown メソッドは、タイトルバーの左クリックイベントが発生したときに実行されるメソッドです。
        /// </summary>
        /// <param name="window">イベントが発生したウィンドウ</param>
        /// <param name="e">マウスボタンイベントの引数</param>
        protected void OnTitleBarMouseLeftButtonDown(Window window, MouseButtonEventArgs e)
        {
            // ダブルクリックで最大化/元に戻す処理
            if (e.ClickCount == 2)
            {
                window.WindowState =
                    window.WindowState == WindowState.Maximized
                    ? WindowState.Normal
                    : WindowState.Maximized;
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                window.DragMove();
            }
        }
    }
}
