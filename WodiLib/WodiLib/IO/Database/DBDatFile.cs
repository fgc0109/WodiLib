// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDatFile.cs
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
    ///     DBファイル
    /// </summary>
    public class DatabaseDatFile : WoditorFileBase<DBDatFilePath, DBDat,
        DBDatFileWriter, DBDatFileReader>
    {
        #region Constants

        #region public

        /// <summary>
        ///     ファイルヘッダ
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static readonly byte[] Header =
        {
            0x00, 0x57, 0x00, 0x00, 0x4F, 0x4C, 0x00, 0x46, 0x4D, 0x00, 0xC1,
        };

        /// <summary>
        ///     ファイルフッタ
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static readonly byte[] Footer =
        {
            0xC1,
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
        public DatabaseDatFile(DBDatFilePath filePath) : base(filePath)
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
        protected override DBDatFileWriter MakeFileWriter(DBDatFilePath filePath)
        {
            ThrowHelper.ValidateArgumentNotNull(filePath is null, nameof(filePath));

            var writer = new DBDatFileWriter(filePath);
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
        protected override DBDatFileReader MakeFileReader(DBDatFilePath filePath)
        {
            ThrowHelper.ValidateArgumentNotNull(filePath is null, nameof(filePath));

            var reader = new DBDatFileReader(filePath, filePath.DbKind);
            return reader;
        }

        #endregion

        #endregion
    }
}
