// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : AttributeTargetsExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.SourceGenerator.Core.Exceptions
{
    /// <summary>
    ///     自動生成コード登録時にHintNameが重複した場合の例外
    /// </summary>
    internal class DuplicateHintNameException : Exception
    {
        public DuplicateHintNameException(string hintName) : base(
            $"名前の重複が見つかりました。 (hintName: {hintName})"
        )
        {
        }
    }
}
