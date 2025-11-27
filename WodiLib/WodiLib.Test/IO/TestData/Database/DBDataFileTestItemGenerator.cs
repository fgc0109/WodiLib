using System.Collections.Generic;
using System.Linq;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.TestData.Database
{
    public static class DBDataFileTestItemGenerator
    {
        public static DBData GenerateUDB0DBData()
        {
            return new DBData(
                new DBDataSettings
                {
                    DataTable = new DatabaseNamedDataTableSettings(
                        new List<IDatabaseNamedDataRowSettings>
                        {
                            new DatabaseNamedDataRowSettings(
                                GetSortedValueList(TestItems.DatabaseFieldValueList.Udb_Type0_Data1)
                            )
                            {
                                DataName = TestItems.DatabaseDataName.Udb_Type0_Data1,
                            },
                            new DatabaseNamedDataRowSettings(
                                GetSortedValueList(TestItems.DatabaseFieldValueList.Udb_Type0_Data2)
                            )
                            {
                                DataName = TestItems.DatabaseDataName.Udb_Type0_Data2,
                            },
                            new DatabaseNamedDataRowSettings(
                                GetSortedValueList(TestItems.DatabaseFieldValueList.Udb_Type0_Data3)
                            )
                            {
                                DataName = TestItems.DatabaseDataName.Udb_Type0_Data3,
                            },
                        }
                    ),
                }
            );
        }

        public static DBData GenerateCDB0DBData()
        {
            return new DBData(
                new DBDataSettings
                {
                    DataTable = new DatabaseNamedDataTableSettings(
                        new List<IDatabaseNamedDataRowSettings>
                        {
                            new DatabaseNamedDataRowSettings(
                                GetSortedValueList(TestItems.DatabaseFieldValueList.Cdb_Type0_Data0)
                            )
                            {
                                DataName = TestItems.DatabaseDataName.Cdb_Type0_Data0,
                            },
                        }
                    ),
                }
            );
        }

        /// <summary>
        ///     Int値 -> String値 の順に並べ替えたリストに変換する。
        /// </summary>
        /// <param name="original">変換元</param>
        /// <returns>ソートした結果（<paramref name="original"/> とは別インスタンス）</returns>
        private static List<DatabaseFieldValue> GetSortedValueList(List<DatabaseFieldValue> original)
        {
            var intList = new List<DatabaseFieldValue>();
            var stringList = new List<DatabaseFieldValue>();

            foreach (var value in original)
            {
                if (value.Type == DatabaseFieldType.Int)
                {
                    intList.Add(value);
                }
                else
                {
                    stringList.Add(value);
                }
            }

            return intList.Added(stringList).ToList();
        }

        #region テスト用ファイル出力処理

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            ("UDB0_データ_001to003_7.dbdata", TestResources.UDB0_1to3DBData),
            ("あいうえお_データ_000to000_a.dbdata", TestResources.CDB0_0to0DBData),
            ("┣ 主人公行動AI_データ_003to018_.dbdata", TestResources.CDB2_3to18DBData),
            ("状態設定_データ_000to023_戦闘不能.dbdata", TestResources.UDB8_0to23DBData),
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
