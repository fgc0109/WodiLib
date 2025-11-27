// ========================================
// Project Name : WodiLib
// File Name    : DatabaseProjectFile.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     XXXDatabase.projectファイル
    /// </summary>
    public class DatabaseProjectFile : WoditorFileBase<DatabaseProjectFilePath, DBProject,
        DBProjectFileWriter, DBProjectFileReader>
    {
        #region Constructors

        #region Required

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public DatabaseProjectFile(DatabaseProjectFilePath filePath) : base(filePath)
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
        protected override DBProjectFileWriter MakeFileWriter(DatabaseProjectFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath))
                );

            var writer = new DBProjectFileWriter(filePath);
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
        protected override DBProjectFileReader MakeFileReader(DatabaseProjectFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath))
                );

            var reader = new DBProjectFileReader(filePath, filePath.DbKind);
            return reader;
        }

        #endregion

        #endregion
    }
}
