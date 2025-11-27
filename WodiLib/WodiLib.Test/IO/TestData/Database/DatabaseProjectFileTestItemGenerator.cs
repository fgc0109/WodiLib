using System.Collections.Generic;
using WodiLib.Database;

namespace WodiLib.Test.IO.TestData.Database
{
    public static class DatabaseProjectFileTestItemGenerator
    {
        #region GenerateDatabase0Project

        public static DBProject GenerateDatabase0Project()
        {
            return new DBProject(
                new DBProjectSettings
                {
                    DbKind = DatabaseKind.User,
                    ProjectTypeList = new DatabaseProjectTypeListSettings(
                        new List<IDatabaseProjectTypeSettings>
                        {
                            TestItems.DatabaseProjectTypeSettings.Udb_Type0,
                            TestItems.DatabaseProjectTypeSettings.Udb_Type1,
                            TestItems.DatabaseProjectTypeSettings.Udb_Type2,
                            TestItems.DatabaseProjectTypeSettings.Udb_Type3,
                        }
                    ),
                }
            );
        }

        #endregion

        #region GenerateCDatabase0Project

        public static DBProject GenerateCDatabase0Project()
        {
            return new DBProject(
                new DBProjectSettings
                {
                    DbKind = DatabaseKind.Changeable,
                    ProjectTypeList = new DatabaseProjectTypeListSettings(
                        new List<IDatabaseProjectTypeSettings>
                        {
                            TestItems.DatabaseProjectTypeSettings.Cdb_Type0,
                            TestItems.DatabaseProjectTypeSettings.Cdb_Type1,
                        }
                    ),
                }
            );
        }

        #endregion

        #region テスト用ファイル出力処理

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            ("Database0.project", TestResources.Database0Project),
            ("CDatabase0.project", TestResources.CDatabase0Project),
            ("Database1.project", TestResources.Database1Project),
            ("CDatabase1.project", TestResources.CDatabase1Project),
            ("SysDatabase1.project", TestResources.SysDatabase1Project),
        };

        /// <summary>
        ///     ファイルを tmp フォルダに出力する。
        /// </summary>
        public static void OutputFile()
        {
            TestDirHelper.OutputFiles(TestFiles);
        }

        /// <summary>
        ///     ファイルを削除する。
        /// </summary>
        public static void DeleteFile()
        {
            TestDirHelper.DeleteFiles(TestFiles);
        }

        #endregion
    }
}
