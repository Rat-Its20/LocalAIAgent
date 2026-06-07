using System.IO;

namespace LocalAIAgentApp.Services
{
    public class DialogService
    {
        /// <summary>
        /// OpenFolderDialog メソッドは、ユーザーにフォルダを選択させるためのダイアログを表示し、選択されたフォルダのパスを返すメソッドです。
        /// </summary>
        /// <param name="currentDirectory">ダイアログが開かれたときの初期ディレクトリのパス。</param>
        /// <returns>選択されたフォルダのパス。選択されなかった場合は空の文字列。</returns>
        public string OpenFolderDialog(string currentDirectory)
        {
            string result = string.Empty;

            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.ShowNewFolderButton = true;

                // 初期ディレクトリが指定されている場合は、そのディレクトリをダイアログの初期ディレクトリとして設定
                if (!string.IsNullOrEmpty(currentDirectory) && Directory.Exists(currentDirectory))
                {
                    fbd.SelectedPath = currentDirectory;
                }
                else
                {
                    fbd.SelectedPath = Environment.GetEnvironmentVariable("SystemDrive");
                }
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    result = fbd.SelectedPath;
                }
            }

            return result;
        }
    }
}
