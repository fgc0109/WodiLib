// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : DocumentCommentHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.SourceGenerator.Operation.Generation.Main
{
    /// <summary>
    ///     ドキュメントコメント用ヘルパー
    /// </summary>
    internal static class DocumentCommentHelper
    {
        public static string EscapeOperatorMark(string src)
        {
            return src.Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");
        }
    }
}
