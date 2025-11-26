// ========================================
// Project Name : WodiLib
// File Name    : FilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [NotNewLine] ファイルパス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [FilePathStringObjectValue]
    [StringValueObjectJsonConvert]
    public partial record FilePath
    {
        #region Constructors

        #region InitializeMethods

        partial void DoConstructorExpansion(string value)
        {
            // フルパスチェック
            //   .NET Standard 2.1 では一部の不正な文字列がパス中に含まれていても例外が発生しない
            //   そのためこのあとチェックのための追加処理を行う
            try
            {
                _ = Path.GetFullPath(value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    ErrorMessage.Unsuitable("ファイル名", $"（パス：{value}）"),
                    ex
                );
            }

            // ファイル名が適切かどうかチェック
            var dirsAndFile = value.Split('\\');
            foreach (var path in dirsAndFile)
            {
                if (path.All(c => c.Equals('.'))) continue;

                var fileName = Path.GetFileName(path);
                if (fileName.IsEmpty()) continue;

                if (fileName.HasInvalidFileNameChars())
                {
                    throw new ArgumentException(
                        ErrorMessage.Unsuitable("ファイル名", $"（パス：{value}）")
                    );
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        #region public

        #region ToString

        /// <inheritdoc/>
        public override string ToString()
        {
            return RawValue;
        }

        #endregion

        #endregion

        #endregion
    }
}
