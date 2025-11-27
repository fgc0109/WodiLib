// ========================================
// Project Name : WodiLib
// File Name    : DatabaseSchemaWriter.cs
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
    ///     <see cref="ReadOnlyDatabaseSchema"/> インスタンスの内容を
    ///     XXXDatabase.Dat、 XXXDatabase.project に出力する書き出しクラス
    /// </summary>
    public class DatabaseSchemaWriter
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

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private readonly object writeLock = new();

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        /// <summary>
        ///     コンストラクタ（<see cref="DatFilePath"/>, <see cref="ProjectFilePath"/> から生成するコンストラクタの統合版）
        /// </summary>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="datFilePath"/>, <paramref name="projectFilePath"/> が <see langword="null"/> の場合。
        /// </exception>
        private DatabaseSchemaWriter(
            DBDatFilePath datFilePath,
            DatabaseProjectFilePath projectFilePath
        )
        {
            ThrowHelper.ValidateArgumentNotNull(datFilePath is null, nameof(datFilePath));
            ThrowHelper.ValidateArgumentNotNull(projectFilePath is null, nameof(projectFilePath));

            DatFilePath = datFilePath;
            ProjectFilePath = projectFilePath;
        }

        #endregion

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="datFilePath"/>, <paramref name="projectFilePath"/> が <see langword="null"/> の場合。
        /// </exception>
        public DatabaseSchemaWriter(
            ChangeableDatabaseDatFilePath datFilePath,
            ChangeableDatabaseProjectFilePath projectFilePath
        ) : this(
            datFilePath,
            (DatabaseProjectFilePath)projectFilePath
        )
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="datFilePath"/>, <paramref name="projectFilePath"/> が <see langword="null"/> の場合。
        /// </exception>
        public DatabaseSchemaWriter(
            UserDatabaseDatFilePath datFilePath,
            UserDatabaseProjectFilePath projectFilePath
        ) : this(
            datFilePath,
            (DatabaseProjectFilePath)projectFilePath
        )
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="datFilePath"/>, <paramref name="projectFilePath"/> が <see langword="null"/> の場合。
        /// </exception>
        public DatabaseSchemaWriter(
            SystemDatabaseDatFilePath datFilePath,
            SystemDatabaseProjectFilePath projectFilePath
        ) : this(
            datFilePath,
            (DatabaseProjectFilePath)projectFilePath
        )
        {
        }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <summary>
        ///     ファイルを同期的に書き出す。
        /// </summary>
        /// <param name="data">出力データ</param>
        /// <returns>書き出しTask</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="data"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     ファイル名が正しくない場合、
        ///     または <see cref="DatFilePath"/>, <see cref="ProjectFilePath"/> が非ファイルデバイスを参照している場合
        /// </exception>
        public void WriteSync(ReadOnlyDatabaseSchema data)
        {
            lock (writeLock)
            {
                WodiLibLogger.Info(FileIOMessage.StartFileWrite(GetType()));

                // 出力データ変換
                var divideResult = DatabaseSchemaDivider.Divide(data);

                var writeDatabaseDat = new DBDat(
                    new DBDatSettings
                    {
                        DbKind = data.DbKind,
                        DataTableDefinitionList = divideResult.DataTableList,
                    }
                );

                var writeDatabaseProject = new DBProject(
                    new DBProjectSettings
                    {
                        DbKind = data.DbKind,
                        ProjectTypeList = divideResult.TypeList,
                    }
                );

                // ファイル出力
                var datFileWriter = new DBDatFileWriter(DatFilePath);
                datFileWriter.WriteSync(writeDatabaseDat);

                var projectFileWriter = new DBProjectFileWriter(ProjectFilePath);
                projectFileWriter.WriteSync(writeDatabaseProject);

                WodiLibLogger.Info(FileIOMessage.EndFileWrite(GetType()));
            }
        }

        /// <summary>
        ///     ファイルを非同期的に書き出す。
        /// </summary>
        /// <param name="data">出力データ</param>
        /// <returns>書き出しTask</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="data"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     ファイル名が正しくない場合、
        ///     または <see cref="DatFilePath"/>, <see cref="ProjectFilePath"/> が非ファイルデバイスを参照している場合
        /// </exception>
        public async Task WriteAsync(ReadOnlyDatabaseSchema data)
        {
            await Task.Run(() => WriteSync(data));
        }

        #endregion

        #endregion
    }
}
