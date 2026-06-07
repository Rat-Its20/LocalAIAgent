using Autofac;
using Autofac.Extensions.DependencyInjection;
using LocalAIAgentApp.AI;
using LocalAIAgentApp.Model.Services;       
using LocalAIAgentApp.Services;
using LocalAIAgentApp.ViewModels;
using LocalAIAgentApp.ViewModels.Base;
using LocalAIAgentApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Application = System.Windows.Application;

namespace LocalAIAgentApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// IContainer は、コンポーネントを格納するためのインターフェイスです。
        /// アプリケーション全体で共有されるコンテナを定義しています。
        /// </summary>
        public IContainer _container { get; private set; }

        /// <summary>
        /// OnStartup メソッドは、アプリケーションの起動時に呼び出されるイベントハンドラーです。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // 依存性注入のセットアップ
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);

            // Autofac を使用してサービスを登録し、コンテナを構築します。
            ContainerBuilder builder = new ContainerBuilder();
            builder.Populate(services);
            ConfigureViews(builder);
            _container = builder.Build();

            // アプリケーションのメインウィンドウを表示します。
            using (ILifetimeScope scope = _container.BeginLifetimeScope())
            {
                MainWindow mainWindow = scope.Resolve<MainWindow>();
                mainWindow.DataContext = scope.Resolve<MainWindowViewModel>();
                mainWindow.Show();
            }

            base.OnStartup(e);
        }

        /// <summary>
        /// ServiceCollection を使用して、アプリケーションで使用されるサービスや依存関係を登録するためのメソッドです。
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureServices(IServiceCollection services)
        {
            // LocalAIAgentApp
            services.AddSingleton<DialogService>();

            // LocalAIAgentApp.AI
            services.AddSingleton<FoundryLocalFacade>();

            // LocalAIAgentApp.Model
            services.AddSingleton<FileService>();
        }

        /// <summary>
        /// Views を ContainerBuilder に登録するためのメソッドです。
        /// これにより、依存性注入を使用して ViewModel を Views に提供できるようになります。
        /// </summary>
        /// <param name="builder"></param>
        private void ConfigureViews(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<ModelSettingWindow>().AsSelf();

            builder.RegisterType<ModelSettingWindowViewModel>().AsSelf().As<ViewModelBase>().SingleInstance();
            builder.RegisterType<MainWindowViewModel>().AsSelf().As<ViewModelBase>().SingleInstance();
        }
    }
}
