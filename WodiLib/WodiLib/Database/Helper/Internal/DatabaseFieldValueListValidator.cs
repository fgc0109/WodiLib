// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldValueListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Database
{
    internal class DatabaseFieldValueListValidator :
        RestrictedCapacityListValidator<IDatabaseFieldValueListSettings, DatabaseFieldValue>
    {
        public DatabaseFieldType FieldType { get; }

        public DatabaseFieldValueListValidator(
            GetCountDelegate countGetter,
            GetMaxCapacityDelegate maxCapacityGetter,
            GetMinCapacityDelegate minCapacityGetter,
            DatabaseFieldType fieldType
        ) : base(countGetter, maxCapacityGetter, minCapacityGetter)
        {
            FieldType = fieldType;
        }

        public override void Constructor(NamedValue<IDatabaseFieldValueListSettings> initSettings)
        {
            base.Constructor(initSettings);

            FieldValueListValidationHelper.ValidateUnifiedFieldType(
                (initSettings.Name, initSettings.Value.Settings),
                FieldType
            );
        }

        public override void Set(NamedValue<int> index, NamedValue<IEnumerable<DatabaseFieldValue>> items)
        {
            base.Set(index, items);

            FieldValueListValidationHelper.ValidateUnifiedFieldType(items, FieldType);
        }

        public override void Insert(NamedValue<int> index, NamedValue<IEnumerable<DatabaseFieldValue>> items)
        {
            base.Insert(index, items);

            FieldValueListValidationHelper.ValidateUnifiedFieldType(items, FieldType);
        }

        public override void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<DatabaseFieldValue>> items)
        {
            base.Overwrite(index, items);

            FieldValueListValidationHelper.ValidateUnifiedFieldType(items, FieldType);
        }

        public override void Reset(NamedValue<IEnumerable<DatabaseFieldValue>> items, bool canChangeSize = true)
        {
            base.Reset(items, canChangeSize);

            FieldValueListValidationHelper.ValidateUnifiedFieldType(items, FieldType);
        }
    }
}
