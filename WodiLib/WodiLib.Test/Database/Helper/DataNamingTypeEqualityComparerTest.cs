using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DataNamingTypeEqualityComparerTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Equals

        private static readonly object[][] EqualsTestCaseSource =
        {
            // [leftPatternName, rightPatternName, expected]
            new object[] { CompareItem.Manual.PatternName, CompareItem.Manual.PatternName, true },
            new object[] { CompareItem.Manual.PatternName, CompareItem.FirstStringData.PatternName, false },
            new object[] { CompareItem.Manual.PatternName, CompareItem.EqualBefore.PatternName, false },
            new object[] { CompareItem.Manual.PatternName, CompareItem.DesignatedTypeWithNull.PatternName, false },
            new object[] { CompareItem.Manual.PatternName, CompareItem.DesignatedTypeChangeable1.PatternName, false },
            new object[] { CompareItem.Manual.PatternName, CompareItem.DesignatedTypeChangeable20.PatternName, false },
            new object[] { CompareItem.Manual.PatternName, CompareItem.DesignatedTypeSystem1.PatternName, false },
            new object[]
                { CompareItem.Manual.PatternName, CompareItem.DesignatedTypeSystem1OtherInstance.PatternName, false },
            new object[] { CompareItem.Manual.PatternName, CompareItem.DesignatedTypeUser1.PatternName, false },
            new object[] { CompareItem.FirstStringData.PatternName, CompareItem.FirstStringData.PatternName, true },
            new object[] { CompareItem.EqualBefore.PatternName, CompareItem.EqualBefore.PatternName, true },
            new object[]
            {
                CompareItem.DesignatedTypeWithNull.PatternName, CompareItem.DesignatedTypeWithNull.PatternName, true,
            },
            new object[]
            {
                CompareItem.DesignatedTypeWithNull.PatternName, CompareItem.DesignatedTypeChangeable1.PatternName,
                false,
            },
            new object[]
            {
                CompareItem.DesignatedTypeWithNull.PatternName, CompareItem.DesignatedTypeSystem1.PatternName, false,
            },
            new object[]
                { CompareItem.DesignatedTypeWithNull.PatternName, CompareItem.DesignatedTypeUser1.PatternName, false },
        };

        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void EqualsTest_Success(string leftPatternName, string rightPatternName, bool expected)
        {
            var left = CompareItem.Get(leftPatternName);
            var right = CompareItem.Get(rightPatternName);

            pureFunctionTestHelper.PureFuncSuccess(
                instance: new DataNamingTypeEqualityComparer(),
                execFunc: target => target.Equals(left, right),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(expected)
            );
        }

        #endregion

        #region TestData

        public record CompareItem(
            string PatternName,
            DatabaseDataNamingType DataNamingType,
            DataNameSpecificationDefinition? SpecialDefinition
        )
        {
            public static readonly CompareItem Manual = new(
                nameof(Manual),
                DatabaseDataNamingType.Manual,
                null
            );

            public static readonly CompareItem FirstStringData = new(
                nameof(FirstStringData),
                DatabaseDataNamingType.FirstStringData,
                null
            );

            public static readonly CompareItem EqualBefore = new(
                nameof(EqualBefore),
                DatabaseDataNamingType.EqualBefore,
                null
            );

            public static readonly CompareItem DesignatedTypeWithNull = new(
                nameof(DesignatedTypeWithNull),
                DatabaseDataNamingType.DesignatedType,
                null
            );

            public static readonly CompareItem DesignatedTypeChangeable1 = new(
                nameof(DesignatedTypeChangeable1),
                DatabaseDataNamingType.DesignatedType,
                new DataNameSpecificationDefinition
                {
                    DatabaseKind = DatabaseKind.Changeable,
                    TypeId = 1,
                }
            );

            public static readonly CompareItem DesignatedTypeChangeable20 = new(
                nameof(DesignatedTypeChangeable20),
                DatabaseDataNamingType.DesignatedType,
                new DataNameSpecificationDefinition
                {
                    DatabaseKind = DatabaseKind.Changeable,
                    TypeId = 20,
                }
            );

            public static readonly CompareItem DesignatedTypeSystem1 = new(
                nameof(DesignatedTypeSystem1),
                DatabaseDataNamingType.DesignatedType,
                new DataNameSpecificationDefinition
                {
                    DatabaseKind = DatabaseKind.System,
                    TypeId = 1,
                }
            );

            public static readonly CompareItem DesignatedTypeSystem1OtherInstance = new(
                nameof(DesignatedTypeSystem1OtherInstance),
                DatabaseDataNamingType.DesignatedType,
                new DataNameSpecificationDefinition
                {
                    DatabaseKind = DatabaseKind.System,
                    TypeId = 1,
                }
            );

            public static readonly CompareItem DesignatedTypeUser1 = new(
                nameof(DesignatedTypeUser1),
                DatabaseDataNamingType.DesignatedType,
                new DataNameSpecificationDefinition
                {
                    DatabaseKind = DatabaseKind.User,
                    TypeId = 1,
                }
            );

            public (DatabaseDataNamingType namingType, Func<DataNameSpecificationDefinition?> definition) ToTuple()
                => (DataNamingType, () => SpecialDefinition);

            public static (DatabaseDataNamingType namingType, Func<DataNameSpecificationDefinition?> definition) Get(
                string patternName
            )
            {
                if (patternName == Manual.PatternName)
                {
                    return Manual.ToTuple();
                }

                if (patternName == FirstStringData.PatternName)
                {
                    return FirstStringData.ToTuple();
                }

                if (patternName == EqualBefore.PatternName)
                {
                    return EqualBefore.ToTuple();
                }

                if (patternName == DesignatedTypeWithNull.PatternName)
                {
                    return DesignatedTypeWithNull.ToTuple();
                }

                if (patternName == DesignatedTypeChangeable1.PatternName)
                {
                    return DesignatedTypeChangeable1.ToTuple();
                }

                if (patternName == DesignatedTypeChangeable20.PatternName)
                {
                    return DesignatedTypeChangeable20.ToTuple();
                }

                if (patternName == DesignatedTypeSystem1.PatternName)
                {
                    return DesignatedTypeSystem1.ToTuple();
                }

                if (patternName == DesignatedTypeSystem1OtherInstance.PatternName)
                {
                    return DesignatedTypeSystem1OtherInstance.ToTuple();
                }

                if (patternName == DesignatedTypeUser1.PatternName)
                {
                    return DesignatedTypeUser1.ToTuple();
                }

                throw new InvalidOperationException();
            }
        }

        #endregion
    }
}
