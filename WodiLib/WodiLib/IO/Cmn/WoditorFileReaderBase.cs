// ========================================
// Project Name : WodiLib
// File Name    : WoditorFileReaderBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    // TODO: ファイル全体をメモリに読み込んでいるが、Streamで扱うようにする

    /// <summary>
    ///     ウディタ関連ファイル読み込み基底クラス
    /// </summary>
    /// <typeparam name="TFilePath">ファイルパス</typeparam>
    /// <typeparam name="TFileData">読み込み結果クラス</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class WoditorFileReaderBase<TFilePath, TFileData>
        where TFilePath : FilePath
    {
        /// <summary>読み込みファイルパス</summary>
        public TFilePath FilePath { get; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="filePath">読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="filePath"/> が <see langword="null"/> の場合。
        /// </exception>
        protected WoditorFileReaderBase(TFilePath filePath)
        {
            ThrowHelper.ValidateArgumentNotNull(filePath is null, nameof(filePath));

            FilePath = filePath;
        }

        /// <summary>
        ///     ファイルを同期的に読み込む。
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <exception cref="BinaryFormatterException">
        ///     ファイルが正しく読み込めなかった場合。
        /// </exception>
        public abstract TFileData ReadSync();

        /// <summary>
        ///     ファイルを非同期的に読み込む。
        /// </summary>
        /// <returns>読み込み成否</returns>
        /// <exception cref="BinaryFormatterException">
        ///     ファイルが正しく読み込めなかった場合。
        /// </exception>
        public async Task<TFileData> ReadAsync()
        {
            return await Task.Run(ReadSync);
        }
    }
}
