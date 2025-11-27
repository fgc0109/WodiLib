using System.Collections.Generic;
using System.IO;

namespace WodiLib.Test.IO
{
    internal static class TestDirHelper
    {
        private static readonly object createTestWorkDirLock = new();

        /// <summary>
        ///     テストディレクトリルートフォルダが存在しない場合作成する。
        /// </summary>
        public static void CreateTestWorkRootDirIfNeed()
        {
            lock (createTestWorkDirLock)
            {
                if (!Directory.Exists(IoTestDataConstants.TestWorkRootDir))
                {
                    Directory.CreateDirectory(IoTestDataConstants.TestWorkRootDir);
                }
            }
        }

        /// <summary>
        ///     テストディレクトリルートフォルダ配下に指定したフォルダが存在しない場合作成する。
        /// </summary>
        public static void CreateDirIfNeed(string relativePath)
        {
            CreateTestWorkRootDirIfNeed();

            var dirPath = $@"{IoTestDataConstants.TestWorkRootDir}\{relativePath}";
            lock (createTestWorkDirLock)
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }
        }

        /// <summary>
        ///     ファイルを tmp フォルダに出力する。
        /// </summary>
        public static void OutputFiles(IEnumerable<(string, byte[])> files)
        {
            CreateTestWorkRootDirIfNeed();

            foreach (var (fileName, bytes) in files)
            {
                using var fs = new FileStream(MakeFileFullPath(fileName), FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        ///     ファイルを削除する。
        /// </summary>
        public static void DeleteFiles(IEnumerable<(string, byte[])> files)
        {
            foreach (var (fileName, _) in files)
            {
                var fileFullPath = MakeFileFullPath(fileName);
                if (!File.Exists(fileFullPath)) continue;
                try
                {
                    File.Delete(fileFullPath);
                }
                catch
                {
                    // 削除に失敗しても何もしない
                }
            }
        }

        private static string MakeFileFullPath(string fileName)
        {
            return $@"{IoTestDataConstants.TestWorkRootDir}\{fileName}";
        }
    }
}
