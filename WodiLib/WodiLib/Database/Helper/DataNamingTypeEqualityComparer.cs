// ========================================
// Project Name : WodiLib
// File Name    : DataNamingTypeEqualityComparer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Database
{
    /// <summary>
    ///     <see cref="DatabaseDataNamingType"/> および <see cref="DataNameSpecificationDefinition"/> の比較処理定義クラス。
    /// </summary>
    public class DataNamingTypeEqualityComparer : IEqualityComparer<(DatabaseDataNamingType namingType,
        Func<DataNameSpecificationDefinition?> definition)>
    {
        /// <summary>
        ///     シングルトンインスタンス
        /// </summary>
        public static readonly DataNamingTypeEqualityComparer Instance = new();

        /// <summary>
        ///     <see cref="DatabaseDataNamingType"/> および <see cref="DataNameSpecificationDefinition"/> の比較処理。
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns><paramref name="left"/>と<paramref name="right"/>が一致する場合<see langrowd="true"/></returns>
        public bool Equals(
            (DatabaseDataNamingType namingType, Func<DataNameSpecificationDefinition?> definition) left,
            (DatabaseDataNamingType namingType, Func<DataNameSpecificationDefinition?> definition) right
        )
        {
            if (left.namingType != right.namingType) return false;

            // DataNamingType == other.DataNamingType

            if (left.namingType != DatabaseDataNamingType.DesignatedType) return true;

            // DataNamingType == other.DataNamingType == DatabaseDataNamingType.DesignatedType

            var leftDef = left.definition();
            var rightDef = right.definition();

            if (leftDef is null && rightDef is null)
            {
                return true;
            }

            if (leftDef is null || rightDef is null)
            {
                return false;
            }

            return leftDef.Equals(rightDef);
        }

        /// <inheritdoc/>
        public int GetHashCode(
            (DatabaseDataNamingType namingType, Func<DataNameSpecificationDefinition?> definition) obj
        )
        {
            var definition = obj.definition();

            return obj.namingType.GetHashCode()
                   ^ (definition is null
                       ? 0
                       : definition.GetHashCode());
        }
    }
}
