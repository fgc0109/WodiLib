// ========================================
// Project Name : WodiLib
// File Name    : DBTypeSetFile.cs
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
    ///     DBタイプセットファイル
    /// </summary>
    public class DBTypeSetFile : WoditorFileBase<DBTypeSetFilePath, DBTypeSet,
        DBTypeSetFileWriter, DBTypeSetFileReader>
    {
        #region Constants

        #region public

        /// <summary>
        ///     ファイルヘッダ
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static readonly byte[] Header =
        {
            0xB9, 0x22, 0x2D, 0x02,
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
        public DBTypeSetFile(DBTypeSetFilePath filePath) : base(filePath)
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
        protected override DBTypeSetFileWriter MakeFileWriter(DBTypeSetFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath))
                );

            var writer = new DBTypeSetFileWriter(filePath);
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
        protected override DBTypeSetFileReader MakeFileReader(DBTypeSetFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath))
                );

            var reader = new DBTypeSetFileReader(filePath);
            return reader;
        }

        #endregion

        #endregion
    }
}
