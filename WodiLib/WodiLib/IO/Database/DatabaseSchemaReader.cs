// ========================================
// Project Name : WodiLib
// File Name    : DatabaseSchemaReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Threading.Tasks;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    ///     XXXDatabase.Dat ファイルと XXXDatabase.project ファイルをまとめて読み込み
    ///     整合性のある一つのクラスを取得するファイル読み込みクラス
    /// </summary>
    public class DatabaseSchemaReader
    {
        #region Properties

        #region public

        /// <summary>読み込みデータファイルパス</summary>
        public DBDatFilePath DatFilePath { get; }

        /// <summary>読み込みプロジェクトファイルパス</summary>
        public DatabaseProjectFilePath ProjectFilePath { get; }

        #endregion

        #region private

        /// <summary>ロガー</summary>
        private WodiLibLogger WodiLibLogger { get; } = WodiLibLogger.GetInstance();

        /// <summary>DB種別</summary>
        private DatabaseKind DbKind { get; } = null!;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private readonly object readLock = new();

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        /// <summary>
        ///     コンストラクタ（<see cref="DBDatFilePath"/>, <see cref="DatabaseProjectFilePath"/> から生成するコンストラクタの統合版）
        /// </summary>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="datFilePath"/>, <paramref name="projectFilePath"/> が <see langword="null"/> の場合。
        /// </exception>
        private DatabaseSchemaReader(DBDatFilePath datFilePath, DatabaseProjectFilePath projectFilePath)
        {
            ThrowHelper.ValidateArgumentNotNull(datFilePath is null, nameof(datFilePath));
            ThrowHelper.ValidateArgumentNotNull(projectFilePath is null, nameof(projectFilePath));

            DatFilePath = datFilePath;
            ProjectFilePath = projectFilePath;
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="datFilePath"/>, <paramref name="projectFilePath"/> が <see langword="null"/> の場合。
        /// </exception>
        public DatabaseSchemaReader(
            ChangeableDatabaseDatFilePath datFilePath,
            ChangeableDatabaseProjectFilePath projectFilePath
        ) : this(
            datFilePath,
            (DatabaseProjectFilePath)projectFilePath
        )
        {
            DbKind = DatabaseKind.Changeable;
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="datFilePath"/>, <paramref name="projectFilePath"/> が <see langword="null"/> の場合。
        /// </exception>
        public DatabaseSchemaReader(
            UserDatabaseDatFilePath datFilePath,
            UserDatabaseProjectFilePath projectFilePath
        ) : this(
            datFilePath,
            (DatabaseProjectFilePath)projectFilePath
        )
        {
            DbKind = DatabaseKind.User;
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="datFilePath"/>, <paramref name="projectFilePath"/> が <see langword="null"/> の場合。
        /// </exception>
        public DatabaseSchemaReader(
            SystemDatabaseDatFilePath datFilePath,
            SystemDatabaseProjectFilePath projectFilePath
        ) : this(
            datFilePath,
            (DatabaseProjectFilePath)projectFilePath
        )
        {
            DbKind = DatabaseKind.System;
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <summary>
        ///     ファイルを同期的に読み込む。
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <exception cref="BinaryFormatterException">
        ///     ファイルが正しく読み込めなかった場合。
        /// </exception>
        public DatabaseSchema ReadSync()
        {
            lock (readLock)
            {
                WodiLibLogger.Info(FileIOMessage.StartFileRead(GetType()));

                var datFileReader = new DBDatFileReader(DatFilePath, DbKind);
                var dataTableList = datFileReader.ReadSync().DataTableDefinitionList;

                var projectFile = new DBProjectFileReader(ProjectFilePath, DbKind);
                var typeDefList = projectFile.ReadSync().ProjectTypeList;

                var result = DatabaseSchemaFactory.CreateMerged(
                    dataTableList,
                    typeDefList,
                    DbKind
                );

                WodiLibLogger.Info(FileIOMessage.EndFileRead(GetType()));

                return result;
            }
        }

        /// <summary>
        ///     ファイルを非同期的に読み込む。
        /// </summary>
        /// <returns>読み込み成否</returns>
        /// <exception cref="BinaryFormatterException">
        ///     ファイルが正しく読み込めなかった場合。
        /// </exception>
        public async Task<DatabaseSchema> ReadAsync()
        {
            return await Task.Run(ReadSync);
        }

        #endregion

        #endregion
    }
}
