// ========================================
// Project Name : WodiLib
// File Name    : DBTypeFile.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     DBタイプファイル
    /// </summary>
    public class DBTypeFile : WoditorFileBase<DBTypeFilePath, DBType,
        DBTypeFileWriter, DBTypeFileReader>
    {
        #region Constants

        #region public

        /// <summary>
        ///     ヘッダ
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static readonly byte[] Header =
        {
            0x84, 0x2E, 0x77, 0x02,
        };

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="filePath"/> が <see langword="null"/> の場合。
        /// </exception>
        public DBTypeFile(DBTypeFilePath filePath) : base(filePath)
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region protected

        /// <summary>
        ///     ファイル書き出しクラスを生成する。
        /// </summary>
        /// <param name="filePath">書き出しファイル名</param>
        /// <returns>ライターインスタンス</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="filePath"/> が <see langword="null"/> の場合。
        /// </exception>
        protected override DBTypeFileWriter MakeFileWriter(DBTypeFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath))
                );

            var writer = new DBTypeFileWriter(filePath);
            return writer;
        }

        /// <summary>
        ///     ファイル読み込みクラスを生成する。
        /// </summary>
        /// <param name="filePath">読み込みファイル名</param>
        /// <returns>リーダーインスタンス</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="filePath"/> が <see langword="null"/> の場合。
        /// </exception>
        protected override DBTypeFileReader MakeFileReader(DBTypeFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath))
                );

            var reader = new DBTypeFileReader(filePath);
            return reader;
        }

        #endregion

        #endregion
    }
}
