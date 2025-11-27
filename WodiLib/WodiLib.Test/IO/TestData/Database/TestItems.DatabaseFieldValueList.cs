// ========================================
// Project Name : WodiLib.Test
// File Name    : TestItems.DatabaseFieldValueList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Database;

namespace WodiLib.Test.IO.TestData.Database
{
    internal static partial class TestItems
    {
        internal static class DatabaseFieldValueList
        {
            public static readonly List<DatabaseFieldValue> Udb_Type0_Data0;
            public static readonly List<DatabaseFieldValue> Udb_Type0_Data1;
            public static readonly List<DatabaseFieldValue> Udb_Type0_Data2;
            public static readonly List<DatabaseFieldValue> Udb_Type0_Data3;

            public static readonly List<DatabaseFieldValue> Udb_Type1_Data0;

            public static readonly List<DatabaseFieldValue> Udb_Type2_Data0;
            public static readonly List<DatabaseFieldValue> Udb_Type2_Data1;

            public static readonly List<DatabaseFieldValue> Udb_Type3_Data0;

            public static readonly List<DatabaseFieldValue> Cdb_Type0_Data0;
            public static readonly List<DatabaseFieldValue> Cdb_Type0_Data1;
            public static readonly List<DatabaseFieldValue> Cdb_Type0_Data2;

            public static readonly List<DatabaseFieldValue> Cdb_Type1_Data0;

            static DatabaseFieldValueList()
            {
                Udb_Type0_Data0 = new List<DatabaseFieldValue>
                {
                    (DatabaseValueInt)(-255),
                    (DatabaseValueString)"文字列",
                    (DatabaseValueString)"MapChip/[A]World_Grass-Grass_pipo.png",
                    (DatabaseValueString)"Map002.mps",
                    (DatabaseValueInt)0,
                    (DatabaseValueInt)(-2),
                    (DatabaseValueInt)3,
                };
                Udb_Type0_Data1 = new List<DatabaseFieldValue>
                {
                    (DatabaseValueInt)6,
                    (DatabaseValueString)"7",
                    (DatabaseValueString)"MapData/Map002.mps",
                    (DatabaseValueString)"",
                    (DatabaseValueInt)0,
                    (DatabaseValueInt)(-3),
                    (DatabaseValueInt)9,
                };
                Udb_Type0_Data2 = new List<DatabaseFieldValue>
                {
                    (DatabaseValueInt)0,
                    (DatabaseValueString)"うでぃた",
                    (DatabaseValueString)"",
                    (DatabaseValueString)"",
                    (DatabaseValueInt)0,
                    (DatabaseValueInt)0,
                    (DatabaseValueInt)9,
                };
                Udb_Type0_Data3 = new List<DatabaseFieldValue>
                {
                    (DatabaseValueInt)0,
                    (DatabaseValueString)"",
                    (DatabaseValueString)"まっぷでーた",
                    (DatabaseValueString)"Map007.mps",
                    (DatabaseValueInt)0,
                    (DatabaseValueInt)(-1),
                    (DatabaseValueInt)3,
                };

                Udb_Type1_Data0 = new List<DatabaseFieldValue>();

                Udb_Type2_Data0 = new List<DatabaseFieldValue>
                {
                    (DatabaseValueString)"",
                    (DatabaseValueString)"",
                    (DatabaseValueString)"",
                    (DatabaseValueString)"",
                    (DatabaseValueString)"",
                };
                Udb_Type2_Data1 = new List<DatabaseFieldValue>
                {
                    (DatabaseValueString)"",
                    (DatabaseValueString)"",
                    (DatabaseValueString)"",
                    (DatabaseValueString)"",
                    (DatabaseValueString)"",
                };

                Udb_Type3_Data0 = new List<DatabaseFieldValue>
                {
                    (DatabaseValueString)"ウルファール\r\nエディ\r\n夕一",
                    (DatabaseValueInt)33,
                    (DatabaseValueInt)20,
                };

                Cdb_Type0_Data0 = new List<DatabaseFieldValue>
                {
                    (DatabaseValueString)"",
                    (DatabaseValueInt)255,
                    (DatabaseValueString)"",
                    (DatabaseValueString)"CharaChip/[Animal]ChickenTX.png",
                    (DatabaseValueInt)122,
                    (DatabaseValueInt)8,
                    (DatabaseValueInt)6,
                    (DatabaseValueInt)1,
                    (DatabaseValueString)"234",
                };
                Cdb_Type0_Data1 = new List<DatabaseFieldValue>
                {
                    (DatabaseValueString)"aaa",
                    (DatabaseValueInt)255,
                    (DatabaseValueString)"aaa",
                    (DatabaseValueString)"",
                    (DatabaseValueInt)0,
                    (DatabaseValueInt)0,
                    (DatabaseValueInt)0,
                    (DatabaseValueInt)0,
                    (DatabaseValueString)"",
                };
                Cdb_Type0_Data2 = new List<DatabaseFieldValue>
                {
                    (DatabaseValueString)"",
                    (DatabaseValueInt)127,
                    (DatabaseValueString)"",
                    (DatabaseValueString)"",
                    (DatabaseValueInt)4,
                    (DatabaseValueInt)0,
                    (DatabaseValueInt)127,
                    (DatabaseValueInt)1,
                    (DatabaseValueString)"",
                };

                Cdb_Type1_Data0 = new List<DatabaseFieldValue>
                {
                    (DatabaseValueString)"Wolf RPG Editor!",
                };
            }
        }
    }
}
