using WodiLib.Database;

namespace WodiLib.Test.IO.TestData.Database
{
    internal static partial class TestItems
    {
        internal static class DatabaseDataName
        {
            public static readonly DataName Udb_Type0_Data0;
            public static readonly DataName Udb_Type0_Data1;
            public static readonly DataName Udb_Type0_Data2;
            public static readonly DataName Udb_Type0_Data3;

            public static readonly DataName Udb_Type1_Data0;

            public static readonly DataName Udb_Type2_Data0;
            public static readonly DataName Udb_Type2_Data1;

            public static readonly DataName Udb_Type3_Data0;

            public static readonly DataName Cdb_Type0_Data0;
            public static readonly DataName Cdb_Type0_Data1;
            public static readonly DataName Cdb_Type0_Data2;

            public static readonly DataName Cdb_Type1_Data0;

            static DatabaseDataName()
            {
                Udb_Type0_Data0 = "文字列";
                Udb_Type0_Data1 = "7";
                Udb_Type0_Data2 = "うでぃた";
                Udb_Type0_Data3 = "";

                Udb_Type1_Data0 = "";

                Udb_Type2_Data0 = ""; // ウディタ上で「×NoData」と表示される場合、空文字が格納されている
                Udb_Type2_Data1 = "";

                Udb_Type3_Data0 = "";

                Cdb_Type0_Data0 = "a";
                Cdb_Type0_Data1 = "b";
                Cdb_Type0_Data2 = "c";

                Cdb_Type1_Data0 = "";
            }
        }
    }
}
