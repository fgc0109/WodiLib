// ========================================
// Project Name : WodiLib
// File Name    : WoditorFileBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     ウディタファイル基底クラス
    /// </summary>
    /// <typeparam name="TFilePath">ファイルパス</typeparam>
    /// <typeparam name="TFileData">ファイルデータ</typeparam>
    /// <typeparam name="TWriter">Writer</typeparam>
    /// <typeparam name="TReader">Reader</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class WoditorFileBase<TFilePath, TFileData, TWriter, TReader>
        where TFilePath : FilePath
        where TWriter : WoditorFileWriterBase<TFilePath, TFileData>
        where TReader : WoditorFileReaderBase<TFilePath, TFileData>
    {
        #region Properties

        #region public

        /// <summary>
        ///     ファイルパス
        /// </summary>
        [NotNull]
        public TFilePath FilePath { get; }

        #endregion

        #region private

        private SemaphoreSlim Sem { get; } = new(1, 1);

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="filePath"/> が <see langword="null"/> の場合。
        /// </exception>
        public WoditorFileBase(TFilePath filePath)
        {
            ThrowHelper.ValidateArgumentNotNull(filePath is null, nameof(filePath));

            FilePath = filePath;
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <summary>
        ///     ファイルを同期的に書き出す。
        /// </summary>
        /// <param name="data">書き出しデータ</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="data"/> が <see langword="null"/> の場合。
        /// </exception>
        public void WriteSync(TFileData data)
        {
            ThrowHelper.ValidateArgumentNotNull(data is null, nameof(data));

            Sem.Wait();
            try
            {
                var writer = BuildFileWriter(FilePath);
                writer.WriteSync(data);
            }
            finally
            {
                Sem.Release();
            }
        }

        /// <summary>
        ///     ファイルを非同期的に書き出す。
        /// </summary>
        /// <param name="data">書き出しデータ</param>
        /// <returns>非同期処理タスク</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="data"/> が <see langword="null"/> の場合。
        /// </exception>
        public async Task WriteAsync(TFileData data)
        {
            if (data is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data))
                );

            await Sem.WaitAsync().ConfigureAwait(false);
            try
            {
                var writer = BuildFileWriter(FilePath);
                await writer.WriteAsync(data);
            }
            finally
            {
                Sem.Release();
            }
        }

        /// <summary>
        ///     ファイルを同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータ</returns>
        public TFileData ReadSync()
        {
            Sem.Wait();
            try
            {
                var reader = BuildFileReader(FilePath);
                var result = reader.ReadSync();

                return result;
            }
            finally
            {
                Sem.Release();
            }
        }

        /// <summary>
        ///     ファイルを非同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータを返すタスク</returns>
        public async Task<TFileData> ReadAsync()
        {
            await Sem.WaitAsync().ConfigureAwait(false);
            try
            {
                var reader = BuildFileReader(FilePath);
                var result = await reader.ReadAsync();

                return result;
            }
            finally
            {
                Sem.Release();
            }
        }

        #endregion

        #region protected

        /// <summary>
        ///     ファイル書き出しクラスを生成する。
        /// </summary>
        /// <param name="filePath">書き出しファイル名</param>
        /// <returns>ライターインスタンス</returns>
        protected abstract TWriter MakeFileWriter(TFilePath filePath);

        /// <summary>
        ///     ファイル読み込みクラスを生成する。
        /// </summary>
        /// <param name="filePath">読み込みファイル名</param>
        /// <returns>リーダーインスタンス</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="filePath"/> が <see langword="null"/> の場合。
        /// </exception>
        protected abstract TReader MakeFileReader(TFilePath filePath);

        #endregion

        #region private

        /// <summary>
        ///     ファイル書き出しクラスを生成する。
        /// </summary>
        /// <param name="filePath">書き出しファイル名</param>
        /// <returns>ライターインスタンス</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="filePath"/> が <see langword="null"/> の場合。
        /// </exception>
        private TWriter BuildFileWriter(TFilePath filePath)
        {
            ThrowHelper.ValidateArgumentNotNull(filePath is null, nameof(filePath));

            var writer = MakeFileWriter(filePath);
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
        private TReader BuildFileReader(TFilePath filePath)
        {
            ThrowHelper.ValidateArgumentNotNull(filePath is null, nameof(filePath));

            var reader = MakeFileReader(filePath);
            return reader;
        }

        #endregion

        #endregion
    }
}
