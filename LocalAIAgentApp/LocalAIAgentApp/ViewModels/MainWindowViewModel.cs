using LocalAIAgentApp.Model.Services;
using LocalAIAgentApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalAIAgentApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Field：Modelが存在するかどうかを示すフラグです。
        /// </summary>
        private bool _isModel { get; set; } = true;

        /// <summary>
        /// Service：FileService
        /// </summary>
        private FileService _fileService { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel(FileService fileService) : base()
        {
            // Service を ViewModel に注入します。
            _fileService = fileService;
            _isModel = _fileService.InitializeFolderConfigure(AppDomain.CurrentDomain.BaseDirectory);
        }

        public override void Initialize()
        {
            // Initialize logic here
        }

        public override void SetSubscribe()
        {
            // SetSubscribe logic here
        }
    }
}
