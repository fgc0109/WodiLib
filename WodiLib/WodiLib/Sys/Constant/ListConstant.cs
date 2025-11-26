namespace WodiLib.Sys
{
    /// <summary>
    ///     リスト定数クラス
    /// </summary>
    public static class ListConstant
    {
        /// <summary>
        ///     要素が変更された際のプロパティ変更通知に使用するプロパティ名
        /// </summary>
        public static string IndexerName { get; } = "Item[]";

        /// <summary>
        ///     プロパティを持つリストをJSONシリアライズする際、要素配列に付与するフィールド名
        /// </summary>
        public static string JsonFieldNameItems { get; } = "Items";
    }
}
