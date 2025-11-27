using WodiLib.Database;

namespace WodiLib.Test.IO.TestData.Database
{
    internal static partial class TestItems
    {
        internal static class DataNamingDefinition
        {
            public static readonly DatabaseDataNamingDefinition Udb_Type0;
            public static readonly DatabaseDataNamingDefinition Udb_Type1;
            public static readonly DatabaseDataNamingDefinition Udb_Type2;
            public static readonly DatabaseDataNamingDefinition Udb_Type3;

            public static readonly DatabaseDataNamingDefinition Cdb_Type0;
            public static readonly DatabaseDataNamingDefinition Cdb_Type1;

            static DataNamingDefinition()
            {
                Udb_Type0 = new DatabaseDataNamingDefinition(DatabaseDataNamingType.FirstStringData);
                Udb_Type1 = new DatabaseDataNamingDefinition(DatabaseDataNamingType.Manual);
                Udb_Type2 = new DatabaseDataNamingDefinition(
                    DatabaseDataNamingType.DesignatedType,
                    DatabaseKind.User,
                    4
                );
                Udb_Type3 = new DatabaseDataNamingDefinition(DatabaseDataNamingType.EqualBefore);

                Cdb_Type0 = new DatabaseDataNamingDefinition(DatabaseDataNamingType.Manual);
                Cdb_Type1 = new DatabaseDataNamingDefinition(
                    DatabaseDataNamingType.DesignatedType,
                    DatabaseKind.Changeable,
                    4
                );
            }
        }
    }
}
