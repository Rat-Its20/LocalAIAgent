using LocalAIAgentApp.Model.Base;
using LocalAIAgentApp.Model.Entity.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;

namespace LocalAIAgentApp.Model.Services
{
    public class FileService : ServiceBase
    {
        /// <summary>
        /// exePath プロパティは、実行可能ファイルのパスを格納するためのプロパティです。
        /// </summary>
        private string _exePath { get; set; }

        /// <summary>
        /// filePaths プロパティは、ファイルの種類とそのパスを格納するための辞書です。
        /// </summary>
        private Dictionary<FileType, string> _filePaths { get; set; } = new Dictionary<FileType, string>();

        /// <summary>
        /// jsonSerializerOptions プロパティは、JSON シリアライズのオプションを格納するためのプロパティです。
        /// </summary>
        private JsonSerializerOptions _jsonSerializerOptions { get; set; } = new JsonSerializerOptions
        {
            WriteIndented = true, // JSON をインデントして書き込む
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase // プロパティ名を camelCase に変換する
        };

        #region << Enum >>
        /// <summary>
        /// FoloderType 列挙型は、システムフォルダの種類を定義するための列挙型です。
        /// </summary>
        private enum FolderType
        {
            // SystemFolder: システムフォルダ
            Settings
        }

        private enum FileType
        {
            // AppSettings: アプリケーションの設定ファイル
            [Description("AppSettings.json")]
            AppSettings
        }
        #endregion << Enum >>

        /// <summary>
        /// Constructor
        /// </summary>
        public FileService() : base()
        {
            Debug.WriteLine("FileService initialized");
        }

        /// <summary>
        /// Initialize メソッドは、FileService の初期化を行うためのメソッドです。
        /// </summary>
        public override void Initialize()
        {
            Debug.WriteLine("FileService Initialize method called");
        }

        /// <summary>
        /// InitializeFolderConfigure メソッドは、実行可能ファイルのパスを設定するためのメソッドです。
        /// </summary>
        /// <param name="exePath">実行可能ファイルのパス</param>
        /// <returns>モデルが存在するかどうかを示すフラグ</returns>
        public bool InitializeFolderConfigure(string exePath)
        {
            // モデルが存在するかどうかを示すフラグを初期化します。
            bool isModel = false;

            // 初期化
            AppSettings appSettings = new AppSettings();

            // exePath を設定します。
            _exePath = exePath;

            // exePath を設定した後、必要なフォルダを作成します。
            string settingFolderPath = CreateFolder(Path.Combine(_exePath, FolderType.Settings.ToString()));

            // AppSettings.json
            {
                // filePaths に AppSettings.json のパスを追加します。
                _filePaths[FileType.AppSettings] = Path.Combine(settingFolderPath, FileType.AppSettings.GetDescription());

                // AppSettings.json が存在しない場合は、AppSettings クラスのインスタンスを JSON 形式で AppSettings.json に書き込みます。
                if (!File.Exists(_filePaths[FileType.AppSettings]))
                {
                    WriteJson(new AppSettings(), FileType.AppSettings);
                }

                // AppSettings.json を読み取ります。
                appSettings = ReadJson<AppSettings>(FileType.AppSettings);
            }

            // AppSettings クラスの ModelFolderPath プロパティが null または空でない場合は、モデルが存在するかどうかを確認します。
            {
                if (Path.Exists(appSettings.ModelFolderPath))
                {
                    isModel = true;
                }                
            }

            return isModel;
        }

        #region << File >>
        /// <summary>
        /// CreateFolder メソッドは、指定されたフォルダパスが存在しない場合にディレクトリを作成するためのメソッドです。
        /// </summary>
        /// <param name="folderPath">作成するフォルダのパス</param>
        /// <returns>作成されたフォルダのパス</returns>
        private string CreateFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                // folderPath が存在しない場合は、ディレクトリを作成します。
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }
        #endregion << File >>

        #region << Json >>

        /// <summary>
        /// WriteJson メソッドは、指定されたデータを JSON 形式でファイルに書き込むためのメソッドです。
        /// </summary>
        /// <typeparam name="T">Entity モデル</typeparam>
        /// <param name="data">書き込むデータ</param>
        /// <param name="fileType">FileType 列挙型の値</param>
        private void WriteJson<T>(T data, FileType fileType) where T : class
        {
            // filePath を作成します。
            string filePath = _filePaths[fileType];

            // data を JSON 形式にシリアライズします。
            string json = JsonSerializer.Serialize(data, _jsonSerializerOptions);

            // json を filePath に書き込みます。
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// ReadJson メソッドは、指定されたファイルから JSON 形式のデータを読み取り、指定された型のオブジェクトにデシリアライズするためのメソッドです。
        /// </summary>
        /// <typeparam name="T">デシリアライズする型</typeparam>
        /// <param name="fileType">FileType 列挙型の値</param>
        /// <returns>デシリアライズされたオブジェクト</returns>
        private T ReadJson<T>(FileType fileType) where T : class
        {
            // filePath を作成します。
            string filePath = _filePaths[fileType];

            // filePath が存在するかどうかを確認します。
            if (!File.Exists(filePath))
            {
                // filePath が存在しない場合は、null を返します。
                return default(T);
            }

            // filePath から JSON を読み取ります。
            string json = File.ReadAllText(filePath);

            // json を T 型のオブジェクトにデシリアライズします。
            T data = JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);

            // data が null の場合は、default(T) を返します。
            if (data == null)
            {
                return default(T);
            }

            return data;
        }

        #endregion << Json >>
    }
}
